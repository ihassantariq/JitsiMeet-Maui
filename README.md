# Jitsi Meet MAUI

A **.NET MAUI** sample application that integrates the [Jitsi Meet](https://jitsi.org/) video-conferencing SDK on both **Android** and **iOS** through native binding libraries. This project demonstrates how to create C# bindings for a complex native SDK and consume them from a cross-platform .NET MAUI app.

<!-- Add your own screenshots here
![Android Screenshot](docs/screenshot_android.png)
![iOS Screenshot](docs/screenshot_ios.png)
-->

---

## Table of Contents

- [Project Structure](#project-structure)
- [How the Binding Works](#how-the-binding-works)
  - [Android Binding](#android-binding)
  - [iOS Binding](#ios-binding)
- [Native Binaries](#native-binaries)
  - [Android Dependencies](#android-dependencies)
  - [iOS Frameworks](#ios-frameworks)
- [Demo App](#demo-app)
  - [Architecture](#architecture)
  - [Android Implementation](#android-implementation)
  - [iOS Implementation](#ios-implementation)
- [Getting Started](#getting-started)
- [What You Can Customise](#what-you-can-customise)
- [Versions & Upgrading](#versions--upgrading)
- [Troubleshooting](#troubleshooting)
- [License](#license)

---

## Project Structure

```
JitsiMeetMAUI/
├── JitsiMeetMAUI.sln                  # Solution file (3 projects)
│
├── JitsiMeet.Android.Binding/         # Android binding library
│   ├── JitsiMeet.Android.Binding.csproj
│   └── Transforms/
│       ├── Metadata.xml               # Java → C# type fixes
│       ├── EnumFields.xml
│       └── EnumMethods.xml
│
├── JitsiMeet.iOS.Binding/             # iOS binding library
│   ├── JitsiMeet.iOS.Binding.csproj
│   ├── ApiDefinition.cs               # Objective-C → C# API surface
│   └── Structs.cs                     # Native enums (RecordingMode, etc.)
│
├── JitsiMeetDemo/                     # .NET MAUI demo app
│   ├── IJitsiMeetService.cs           # Cross-platform service interface
│   ├── MainPage.xaml / .cs            # UI & join logic
│   ├── MauiProgram.cs                 # DI registration
│   └── Platforms/
│       ├── Android/
│       │   ├── AndroidJitsiMeetService.cs  # Android IJitsiMeetService
│       │   ├── MauiJitsiMeetActivity.cs    # Hosts the Jitsi meeting view
│       │   └── AndroidManifest.xml         # Permissions
│       └── iOS/
│           └── IOSJitsiMeetService.cs      # iOS IJitsiMeetService
│
├── NativeBinaries/                    # Pre-downloaded native SDK files
│   ├── android/
│   │   ├── jitsi-meet-sdk-12.0.0.aar  # Main Jitsi Android SDK
│   │   └── deps/                      # ~30 transitive AAR/JAR deps
│   └── ios/
│       ├── JitsiMeetSDK.xcframework
│       ├── hermes.xcframework
│       ├── WebRTC.xcframework
│       └── GiphyUISDK.xcframework
│
└── pad_icons.py                       # Utility script for icon padding
```

---

## How the Binding Works

### Android Binding

The Android binding project (`JitsiMeet.Android.Binding`) turns the native Java/Kotlin Jitsi Meet SDK AAR into a C# library that .NET MAUI can reference.

#### 1. The `.csproj` — `JitsiMeet.Android.Binding.csproj`

```xml
<TargetFramework>net9.0-android</TargetFramework>

<!-- Use class-parse for Java→C# stub generation -->
<AndroidClassParser>class-parse</AndroidClassParser>
<AndroidCodegenTarget>XAJavaInterop1</AndroidCodegenTarget>

<!-- Suppress warnings for missing transitive Java deps -->
<MSBuildWarningsAsMessages>XA4241;XA4236;XA4242</MSBuildWarningsAsMessages>
```

**Key points:**

| Setting | What it does |
|---|---|
| `AndroidClassParser` = `class-parse` | Parses the `.class` files inside the AAR to auto-generate C# wrappers |
| `AndroidCodegenTarget` = `XAJavaInterop1` | Generates modern Java interop bindings |
| `XA4241` / `XA4236` suppression | The Jitsi SDK has dozens of transitive React Native dependencies. Suppressing these warnings lets us bind **only** the top-level SDK surface without needing every nested Java type resolved at compile time |

**The main AAR reference:**
```xml
<AndroidLibrary Include="..\NativeBinaries\android\jitsi-meet-sdk-12.0.0.aar" />
```

**NuGet packages** provide the AndroidX and Google Play dependencies that the SDK expects at runtime:

- `Xamarin.AndroidX.AppCompat`
- `Xamarin.AndroidX.Fragment`
- `Xamarin.AndroidX.LocalBroadcastManager`
- `Xamarin.AndroidX.Media3.ExoPlayer` (+ Dash, HLS, SmoothStreaming, UI)
- `Xamarin.AndroidX.SwipeRefreshLayout`
- `GoogleGson`
- `Xamarin.GooglePlayServices.Auth`

#### 2. Metadata Transforms — `Transforms/Metadata.xml`

When the Java→C# code generator runs, sometimes the auto-generated return types are wrong. The `Metadata.xml` file fixes them:

```xml
<metadata>
  <!-- Fix the return type of JitsiInitializer.create() -->
  <attr path="/api/package[@name='org.jitsi.meet.sdk']/class[@name='JitsiInitializer']/method[@name='create' and count(parameter)=1 and parameter[1][@type='android.content.Context']]"
        name="managedReturn">Java.Lang.Object</attr>
</metadata>
```

This specific fix changes the generated C# return type of `JitsiInitializer.create(Context)` to `Java.Lang.Object`, because the auto-generator incorrectly inferred a more specific type.

> **Tip**: If you encounter build errors about incorrect types after updating the SDK version, `Metadata.xml` is where you fix them. You can inspect the AAR contents by renaming it to `.zip`, extracting `classes.jar`, and opening it in a decompiler like [www.decompiler.com](https://www.decompiler.com) to see the actual Java signatures.

---

### iOS Binding

The iOS binding project (`JitsiMeet.iOS.Binding`) wraps the native Objective-C `JitsiMeetSDK.xcframework` into a C# library.

#### 1. The `.csproj` — `JitsiMeet.iOS.Binding.csproj`

```xml
<TargetFramework>net9.0-ios</TargetFramework>
<IsBindingProject>true</IsBindingProject>
<AllowUnsafeBlocks>true</AllowUnsafeBlocks>
```

**Native framework references:**
```xml
<NativeReference Include="..\NativeBinaries\ios\JitsiMeetSDK.xcframework">
  <Kind>Framework</Kind>
  <ForceLoad>True</ForceLoad>
</NativeReference>
<NativeReference Include="..\NativeBinaries\ios\hermes.xcframework">
  <Kind>Framework</Kind>
  <ForceLoad>True</ForceLoad>
</NativeReference>
```

`ForceLoad` = `True` ensures all symbols are linked, which is necessary for React Native's runtime class lookups.

#### 2. API Definition — `ApiDefinition.cs`

This is the heart of the iOS binding. Every Objective-C class/protocol you want to use from C# must be declared here. The file binds **6 key types**:

| C# Type | Objective-C Type | Purpose |
|---|---|---|
| `JitsiMeetUserInfo` | `JitsiMeetUserInfo` | User display name, email, avatar |
| `JitsiMeetConferenceOptionsBuilder` | `JitsiMeetConferenceOptionsBuilder` | Builder to configure meeting options |
| `JitsiMeetConferenceOptions` | `JitsiMeetConferenceOptions` | Immutable conference configuration |
| `JitsiMeetViewDelegate` | `JitsiMeetViewDelegate` | Callbacks: joined, terminated, readyToClose, etc. |
| `JitsiMeetView` | `JitsiMeetView` | The actual UIView that renders the meeting |
| `JitsiMeetSDK` | `JitsiMeet` | Singleton for global config & app lifecycle |

**Example — binding the `JitsiMeetView` class:**
```csharp
[BaseType(typeof(UIView))]
interface JitsiMeetView
{
    [NullAllowed, Export("delegate", ArgumentSemantic.Weak)]
    NSObject Delegate { get; set; }

    [Export("join:")]
    void Join([NullAllowed] JitsiMeetConferenceOptions options);

    [Export("leave")]
    void Leave();

    [Export("hangUp")]
    void HangUp();

    // ... more methods
}
```

#### 3. Structs & Enums — `Structs.cs`

Native enums used by the SDK are declared here:

```csharp
[Native]
public enum RecordingMode : long { File, Stream }

[Native]
public enum WebRTCLoggingSeverity : long { Verbose, Info, Warning, Error, None }
```

> **Tip**: To generate `ApiDefinition.cs` from scratch, you can use the [Objective Sharpie](https://learn.microsoft.com/en-us/xamarin/cross-platform/macios/binding/objective-sharpie/) tool on the `.xcframework` headers, then manually clean up the output.

---

## Native Binaries

### Android Dependencies

The Jitsi Meet Android SDK (v12.0.0) has ~30 transitive React Native dependencies that must be present at runtime. These are **not** NuGet packages — they are raw `.aar` / `.jar` files hosted in Jitsi's own Maven repository.

#### Downloading Dependencies with Gradle

The easiest way to resolve and download all transitive dependencies is to use **Microsoft's Xamarin Gradle Dependency script**. This is the same approach documented in the [maui-native-sdk-bindings](https://github.com/ihassantariq/maui-native-sdk-bindings) guide (see **Step 3: Identify and Add All Transitive Dependencies**).

1. **Create a minimal Android Studio project** with an Android Library module. Place the Jitsi AAR in a `libs/` folder and reference it in the module's `build.gradle`.
2. **Apply Microsoft's Gradle script** by adding this line to the module's `build.gradle`:
   ```groovy
   apply from: 'https://raw.githubusercontent.com/xamarin/XamarinComponents/main/Util/AndroidGradleDependencyInfo.gradle'
   ```
3. **Run the task:**
   ```bash
   ./gradlew :{ModuleName}:xamarin
   ```
   This automatically resolves all transitive dependencies, copies the AAR/JAR files into a `xamarin/` folder, and prints the full list of Maven coordinates.
4. **Copy the resolved files** from the `xamarin/` folder into `NativeBinaries/android/deps/`.

> **Tip:** If you have issues with Gradle 9.0, use the [modified Gradle script](https://gist.githubusercontent.com/ihassantariq/00a23cca84ab4e14b66209dbf96dff40/raw/bbe8975429fc231d06ba1438632ade994792fa90/MauiDependencies.gradle) instead.

All resolved files land in `NativeBinaries/android/deps/` and are referenced by the demo app's `.csproj`:
```xml
<AndroidAarLibrary Include="..\NativeBinaries\android\deps\*.aar" />
<AndroidJavaLibrary Include="..\NativeBinaries\android\deps\*.jar" />
```

#### Handling AAR Naming Conflicts

Sometimes two AARs contain the same Java package, causing the build to fail. To fix this, rename the conflicting `.aar` file to `.zip` — this excludes it from linking while keeping it around for reference. Repeat the build until it succeeds.

### iOS Frameworks

The iOS side uses pre-built `.xcframework` bundles placed in `NativeBinaries/ios/`:

| Framework | Purpose |
|---|---|
| `JitsiMeetSDK.xcframework` | Core Jitsi Meet SDK |
| `hermes.xcframework` | Hermes JavaScript engine (React Native runtime) |
| `WebRTC.xcframework` | WebRTC media engine |
| `GiphyUISDK.xcframework` | Giphy integration for in-meeting reactions |

These are downloaded from the official [Jitsi Meet iOS SDK releases](https://github.com/jitsi/jitsi-meet-ios-sdk-releases).

---

## Demo App

### Architecture

The demo app uses a **dependency injection** pattern to abstract platform-specific Jitsi implementations behind a shared interface.

```
IJitsiMeetService              ← Shared interface
├── AndroidJitsiMeetService    ← Android implementation
└── IOSJitsiMeetService        ← iOS implementation
```

**Registration** in `MauiProgram.cs`:
```csharp
#if ANDROID
    builder.Services.AddSingleton<IJitsiMeetService, AndroidJitsiMeetService>();
#elif IOS
    builder.Services.AddSingleton<IJitsiMeetService, IOSJitsiMeetService>();
#endif

builder.Services.AddTransient<MainPage>();
```

The `MainPage` receives the service via constructor injection and calls `JoinMeeting()` when the user taps the button.

### Android Implementation

On Android, joining a meeting requires launching a **separate Activity** (`MauiJitsiMeetActivity`) because the Jitsi SDK takes over the full screen with its own React Native view hierarchy.

**Flow:**

1. `AndroidJitsiMeetService.JoinMeeting()` sets global default options, then starts `MauiJitsiMeetActivity` via an `Intent` with the room name, display name, and email as extras.
2. `MauiJitsiMeetActivity.OnCreate()` creates a `JitsiMeetView`, builds `JitsiMeetConferenceOptions`, calls `_view.Join(options)`, and sets the view as content.
3. A `BroadcastReceiver` listens for `CONFERENCE_TERMINATED` and `READY_TO_CLOSE` actions to automatically finish the activity.
4. The activity implements `IJitsiMeetActivityInterface` and overrides permission methods to ensure camera/mic access works.

**Required Android permissions** in `AndroidManifest.xml`:
```xml
<uses-permission android:name="android.permission.CAMERA" />
<uses-permission android:name="android.permission.RECORD_AUDIO" />
<uses-permission android:name="android.permission.INTERNET" />
<uses-permission android:name="android.permission.FOREGROUND_SERVICE" />
<uses-permission android:name="android.permission.FOREGROUND_SERVICE_CAMERA" />
<uses-permission android:name="android.permission.FOREGROUND_SERVICE_MICROPHONE" />
```

> **Important**: The demo app sets `<EmbedAssembliesIntoApk>true</EmbedAssembliesIntoApk>` in the csproj to disable Fast Deployment. This is **required** — without it, AndroidX Startup throws missing DEX class exceptions at runtime.

### iOS Implementation

On iOS, the `IOSJitsiMeetService` creates a `JitsiMeetView`, wraps it in a `UIViewController`, and presents it modally over the current MAUI page.

**Flow:**

1. `IOSJitsiMeetService.JoinMeeting()` builds conference options using the builder pattern (`JitsiMeetConferenceOptions.FromBuilder`).
2. A `JitsiMeetView` is created and set as the root view of a new `UIViewController`.
3. The view controller is presented full-screen.
4. A `CustomJitsiDelegate` (subclass of `JitsiMeetViewDelegate`) listens for `ConferenceTerminated` and `ReadyToClose` to dismiss the view controller and clean up.

---

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Visual Studio 2022 (17.12+) or JetBrains Rider with .NET MAUI workload
- **For Android**: Android SDK with API 26+ (configured via `dotnet workload install maui-android`)
- **For iOS**: Xcode 15+ on macOS (configured via `dotnet workload install maui-ios`)

### Steps

1. **Clone the repository**
   ```bash
   git clone https://github.com/ihassantariq/JitsiMeet-Maui.git
   cd JitsiMeet-Maui
   ```

2. **Download Android dependencies** (if the `NativeBinaries/android/deps/` folder is empty)
   Use the Gradle-based approach described in the [Android Dependencies](#android-dependencies) section to resolve and download all transitive dependencies.

3. **Build and run**
   ```bash
   # Android
   dotnet build JitsiMeetDemo/JitsiMeetDemo.csproj -f net9.0-android

   # iOS
   dotnet build JitsiMeetDemo/JitsiMeetDemo.csproj -f net9.0-ios
   ```

4. **If the Android build fails** with AAR naming conflicts, rename the conflicting `.aar` to `.zip` and rebuild. See [Handling AAR Naming Conflicts](#handling-aar-naming-conflicts) for details.

---

## What You Can Customise

### 🔗 Server URL

By default, the app connects to the **public** `https://meet.jit.si` server. To use your own self-hosted Jitsi server, change the URL in:

- **Android**: `AndroidJitsiMeetService.cs` (line 17) and `MauiJitsiMeetActivity.cs` (line 78)
- **iOS**: `IOSJitsiMeetService.cs` (line 25)

```csharp
// Change from:
new Java.Net.URL("https://meet.jit.si")
// To:
new Java.Net.URL("https://your-jitsi-server.com")
```

### 🎛️ Feature Flags

Enable or disable meeting features via the builder:

```csharp
.SetFeatureFlag("chat.enabled", true)          // In-meeting chat
.SetFeatureFlag("invite.enabled", true)         // Invite button
.SetFeatureFlag("prejoinpage.enabled", true)    // Pre-join lobby screen
.SetFeatureFlag("welcomepage.enabled", false)   // Jitsi welcome/home page
.SetFeatureFlag("recording.enabled", false)     // Meeting recording
.SetFeatureFlag("pip.enabled", true)            // Picture-in-picture (Android)
```

### 👤 User Info

Pass authenticated user details:

```csharp
var userInfo = new JitsiMeetUserInfo();
userInfo.DisplayName = "John Doe";
userInfo.Email = "john@example.com";
// userInfo.Avatar = new NSUrl("https://..."); // iOS only
```

### 🎨 App Branding

- **App icon**: Replace `Resources/AppIcon/appiconfg.png`
- **Splash screen**: Replace `Resources/Splash/splash.png`
- **Logo on main page**: Replace `Resources/Images/logo.png`
- **App ID**: Change `<ApplicationId>` in `JitsiMeetDemo.csproj`

### 🔐 JWT Authentication

If your Jitsi server requires JWT tokens:

```csharp
// Android (in MauiJitsiMeetActivity.cs):
.SetToken("your-jwt-token")

// iOS (in IOSJitsiMeetService.cs):
builder.Token = "your-jwt-token";
```

---

## Versions & Upgrading

### Current Versions

This project is pinned to the following versions:

| Component | Version | Notes |
|---|---|---|
| **.NET** | 9.0 (`net9.0-android` / `net9.0-ios`) | Target framework for all 3 projects |
| **Jitsi Meet Android SDK** | **12.0.0** | AAR in `NativeBinaries/android/` |
| **Jitsi Meet iOS SDK** | **12.0.0** | xcframework in `NativeBinaries/ios/` |
| **React Native** | 0.77.2 | `react-android` & `hermes-android` from Maven Central |
| **Jitsi React Native modules** | Build `22286284` | All patched RN modules use this Jitsi build tag |
| **Android minimum API** | 24 (binding) / 26 (demo app) | Set in `.csproj` via `SupportedOSPlatformVersion` |
| **iOS minimum version** | 15.0 (demo) / 15.1 (binding) | Set in `.csproj` via `SupportedOSPlatformVersion` |
| **Xamarin.AndroidX.AppCompat** | 1.6.1.6 | |
| **Xamarin.AndroidX.Media3.ExoPlayer** | 1.9.0.1 | |
| **Xamarin.GooglePlayServices.Auth** | 120.7.0.5 | |

### Upgrading to a Newer Jitsi Meet SDK Version

When a new version of the Jitsi Meet SDK is released, here is what you need to do for each platform:

#### Android Upgrade Steps

1. **Download the new AAR**
   - Go to the [Jitsi Maven Repository](https://github.com/jitsi/jitsi-maven-repository/tree/master/releases/org/jitsi/react/jitsi-meet-sdk) and find the new version.
   - Download the `.aar` file and place it in `NativeBinaries/android/`, replacing the old one.
   - Update the AAR filename reference in `JitsiMeet.Android.Binding.csproj`:
     ```xml
     <!-- Change the version number -->
     <AndroidLibrary Include="..\NativeBinaries\android\jitsi-meet-sdk-NEW_VERSION.aar" />
     ```

2. **Update transitive dependencies**
   - Check the new SDK's `pom.xml` in the Jitsi Maven repo to see if dependency versions have changed.
   - Delete the contents of `NativeBinaries/android/deps/` and re-resolve using the Gradle-based approach described in [Android Dependencies](#android-dependencies).

3. **Fix binding errors**
   - Try building the binding project:
     ```bash
     dotnet build JitsiMeet.Android.Binding/JitsiMeet.Android.Binding.csproj
     ```
   - If the build fails with type errors, update `Transforms/Metadata.xml` to fix the new type mismatches. Inspect the new AAR by renaming it to `.zip`, extracting `classes.jar`, and viewing it in [www.decompiler.com](https://www.decompiler.com).

4. **Update NuGet packages**
   - If the new SDK depends on newer AndroidX or Google Play Services versions, update the `<PackageReference>` versions in `JitsiMeet.Android.Binding.csproj`.

5. **Resolve AAR conflicts**
   - Run the demo build. If it fails with naming conflicts, rename the conflicting `.aar` to `.zip` and rebuild.

#### iOS Upgrade Steps

1. **Download new xcframeworks**
   - Go to [jitsi-meet-ios-sdk-releases](https://github.com/jitsi/jitsi-meet-ios-sdk-releases) and download the new version's xcframeworks.
   - Replace the contents of `NativeBinaries/ios/` with the new frameworks (`JitsiMeetSDK.xcframework`, `hermes.xcframework`, `WebRTC.xcframework`, `GiphyUISDK.xcframework`).

2. **Regenerate `ApiDefinition.cs`** (if the API surface changed)
   - Use [Objective Sharpie](https://learn.microsoft.com/en-us/xamarin/cross-platform/macios/binding/objective-sharpie/) to generate a new definition from the framework headers:
     ```bash
     sharpie bind -framework NativeBinaries/ios/JitsiMeetSDK.xcframework/ios-arm64/JitsiMeetSDK.framework -sdk iphoneos
     ```
   - Compare the output with the existing `ApiDefinition.cs` and merge in any new methods, properties, or classes.
   - Update `Structs.cs` if new enums were added.

3. **Build and test**
   ```bash
   dotnet build JitsiMeet.iOS.Binding/JitsiMeet.iOS.Binding.csproj
   dotnet build JitsiMeetDemo/JitsiMeetDemo.csproj -f net9.0-ios
   ```

#### Upgrading .NET Version (e.g. .NET 9 → .NET 10)

1. Update `<TargetFramework>` in all three `.csproj` files (e.g. `net9.0-android` → `net10.0-android`).
2. Update the MAUI workload:
   ```bash
   dotnet workload update
   ```
3. Check if NuGet package versions (AndroidX, Google Play Services) have newer releases compatible with the new .NET version and update accordingly.
4. Rebuild and test both platforms.

---

## Troubleshooting

| Problem | Solution |
|---|---|
| **XA4241 warnings** about missing Java types | These are expected — we only bind the top-level SDK. They are suppressed in the binding `.csproj`. |
| **AAR naming conflicts** at build time | Rename the conflicting `.aar` file to `.zip` and rebuild. See [Handling AAR Naming Conflicts](#handling-aar-naming-conflicts). |
| **AndroidX Startup / missing DEX** crash at runtime | Ensure `<EmbedAssembliesIntoApk>true</EmbedAssembliesIntoApk>` is set in the demo `.csproj`. Do **not** use Fast Deployment. |
| **iOS linker errors** about missing symbols | Ensure all 4 xcframeworks are referenced in the demo `.csproj` (not just the binding project). Check `ForceLoad` is `True`. |
| **Login required on meet.jit.si** | The public server now requires authentication to *create* rooms. Use your own server or log in with Google/GitHub/Facebook. |
| **Camera / mic permissions denied** | The `MainPage.xaml.cs` requests permissions at runtime. Ensure the Android manifest includes the required `uses-permission` entries. |

---

## License

This project is provided as a sample/demo. The [Jitsi Meet SDK](https://github.com/jitsi/jitsi-meet) is licensed under the Apache 2.0 License.
