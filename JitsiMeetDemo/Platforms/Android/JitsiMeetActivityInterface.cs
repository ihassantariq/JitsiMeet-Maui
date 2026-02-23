using System;
using Android.Runtime;
using Java.Interop;

namespace Com.Facebook.React.Modules.Core
{
    [Register("com/facebook/react/modules/core/PermissionListener", "", "")]
    public interface IPermissionListener : IJavaObject, IJavaPeerable
    {
        [Register("onRequestPermissionsResult", "(I[Ljava/lang/String;[I)Z", "GetOnRequestPermissionsResult_IarrayLjava_lang_String_arrayIHandler:Com.Facebook.React.Modules.Core.IPermissionListenerInvoker, JitsiMeetDemo")]
        bool OnRequestPermissionsResult(int requestCode, string[] permissions, int[] grantResults);
    }

    [Register("com/facebook/react/modules/core/PermissionListener", DoNotGenerateAcw = true)]
    internal class IPermissionListenerInvoker : Java.Lang.Object, IPermissionListener
    {
        public IPermissionListenerInvoker(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer) { }

        static Delegate cb_onRequestPermissionsResult;
        static Delegate GetOnRequestPermissionsResult_IarrayLjava_lang_String_arrayIHandler()
        {
            if (cb_onRequestPermissionsResult == null)
                cb_onRequestPermissionsResult = JNINativeWrapper.CreateDelegate((Func<IntPtr, IntPtr, int, IntPtr, IntPtr, bool>)n_OnRequestPermissionsResult);
            return cb_onRequestPermissionsResult;
        }

        static bool n_OnRequestPermissionsResult(IntPtr jnienv, IntPtr native__this, int requestCode, IntPtr native_permissions, IntPtr native_grantResults)
        {
            var __this = global::Java.Lang.Object.GetObject<IPermissionListener>(native__this, JniHandleOwnership.DoNotTransfer);
            var permissions = (string[])JNIEnv.GetArray(native_permissions, JniHandleOwnership.DoNotTransfer, typeof(string));
            var grantResults = (int[])JNIEnv.GetArray(native_grantResults, JniHandleOwnership.DoNotTransfer, typeof(int));
            bool __ret = __this.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            return __ret;
        }

        public bool OnRequestPermissionsResult(int requestCode, string[] permissions, int[] grantResults) => false;
    }

    [Register("com/facebook/react/modules/core/PermissionAwareActivity", "", "")]
    public interface IPermissionAwareActivity : IJavaObject, IJavaPeerable
    {
        [Register("checkPermission", "(Ljava/lang/String;II)I", "GetCheckPermission_Ljava_lang_String_IIHandler:Com.Facebook.React.Modules.Core.IPermissionAwareActivityInvoker, JitsiMeetDemo")]
        int CheckPermission(string permission, int pid, int uid);

        [Register("checkSelfPermission", "(Ljava/lang/String;)I", "GetCheckSelfPermission_Ljava_lang_String_Handler:Com.Facebook.React.Modules.Core.IPermissionAwareActivityInvoker, JitsiMeetDemo")]
        int CheckSelfPermission(string permission);

        [Register("shouldShowRequestPermissionRationale", "(Ljava/lang/String;)Z", "GetShouldShowRequestPermissionRationale_Ljava_lang_String_Handler:Com.Facebook.React.Modules.Core.IPermissionAwareActivityInvoker, JitsiMeetDemo")]
        bool ShouldShowRequestPermissionRationale(string permission);

        [Register("requestPermissions", "([Ljava/lang/String;ILcom/facebook/react/modules/core/PermissionListener;)V", "GetRequestPermissions_arrayLjava_lang_String_ILcom_facebook_react_modules_core_PermissionListener_Handler:Com.Facebook.React.Modules.Core.IPermissionAwareActivityInvoker, JitsiMeetDemo")]
        void RequestPermissions(string[] permissions, int requestCode, IPermissionListener listener);
    }

    [Register("com/facebook/react/modules/core/PermissionAwareActivity", DoNotGenerateAcw = true)]
    internal class IPermissionAwareActivityInvoker : Java.Lang.Object, IPermissionAwareActivity
    {
        public IPermissionAwareActivityInvoker(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer) { }

        public int CheckPermission(string permission, int pid, int uid) => 0;
        public int CheckSelfPermission(string permission) => 0;
        public bool ShouldShowRequestPermissionRationale(string permission) => false;
        public void RequestPermissions(string[] permissions, int requestCode, IPermissionListener listener) { }

        static Delegate cb_checkPermission;
        static Delegate GetCheckPermission_Ljava_lang_String_IIHandler()
        {
            if (cb_checkPermission == null)
                cb_checkPermission = JNINativeWrapper.CreateDelegate((Func<IntPtr, IntPtr, IntPtr, int, int, int>)n_CheckPermission);
            return cb_checkPermission;
        }
        static int n_CheckPermission(IntPtr jnienv, IntPtr native__this, IntPtr native_permission, int pid, int uid)
        {
            var __this = global::Java.Lang.Object.GetObject<IPermissionAwareActivity>(native__this, JniHandleOwnership.DoNotTransfer);
            return __this.CheckPermission(JNIEnv.GetString(native_permission, JniHandleOwnership.DoNotTransfer), pid, uid);
        }

        static Delegate cb_checkSelfPermission;
        static Delegate GetCheckSelfPermission_Ljava_lang_String_Handler()
        {
            if (cb_checkSelfPermission == null)
                cb_checkSelfPermission = JNINativeWrapper.CreateDelegate((Func<IntPtr, IntPtr, IntPtr, int>)n_CheckSelfPermission);
            return cb_checkSelfPermission;
        }
        static int n_CheckSelfPermission(IntPtr jnienv, IntPtr native__this, IntPtr native_permission)
        {
            var __this = global::Java.Lang.Object.GetObject<IPermissionAwareActivity>(native__this, JniHandleOwnership.DoNotTransfer);
            return __this.CheckSelfPermission(JNIEnv.GetString(native_permission, JniHandleOwnership.DoNotTransfer));
        }

        static Delegate cb_shouldShowRequestPermissionRationale;
        static Delegate GetShouldShowRequestPermissionRationale_Ljava_lang_String_Handler()
        {
            if (cb_shouldShowRequestPermissionRationale == null)
                cb_shouldShowRequestPermissionRationale = JNINativeWrapper.CreateDelegate((Func<IntPtr, IntPtr, IntPtr, bool>)n_ShouldShowRequestPermissionRationale);
            return cb_shouldShowRequestPermissionRationale;
        }
        static bool n_ShouldShowRequestPermissionRationale(IntPtr jnienv, IntPtr native__this, IntPtr native_permission)
        {
            var __this = global::Java.Lang.Object.GetObject<IPermissionAwareActivity>(native__this, JniHandleOwnership.DoNotTransfer);
            return __this.ShouldShowRequestPermissionRationale(JNIEnv.GetString(native_permission, JniHandleOwnership.DoNotTransfer));
        }

        static Delegate cb_requestPermissions;
        static Delegate GetRequestPermissions_arrayLjava_lang_String_ILcom_facebook_react_modules_core_PermissionListener_Handler()
        {
            if (cb_requestPermissions == null)
                cb_requestPermissions = JNINativeWrapper.CreateDelegate((Action<IntPtr, IntPtr, IntPtr, int, IntPtr>)n_RequestPermissions);
            return cb_requestPermissions;
        }
        static void n_RequestPermissions(IntPtr jnienv, IntPtr native__this, IntPtr native_permissions, int requestCode, IntPtr native_listener)
        {
            var __this = global::Java.Lang.Object.GetObject<IPermissionAwareActivity>(native__this, JniHandleOwnership.DoNotTransfer);
            var permissions = (string[])JNIEnv.GetArray(native_permissions, JniHandleOwnership.DoNotTransfer, typeof(string));
            var listener = global::Java.Lang.Object.GetObject<IPermissionListener>(native_listener, JniHandleOwnership.DoNotTransfer);
            __this.RequestPermissions(permissions, requestCode, listener);
        }
    }
}

namespace Org.Jitsi.Meet.Sdk
{
    [Register("org/jitsi/meet/sdk/JitsiMeetActivityInterface", "", "")]
    public interface IJitsiMeetActivityInterface : Com.Facebook.React.Modules.Core.IPermissionAwareActivity, AndroidX.Core.App.ActivityCompat.IOnRequestPermissionsResultCallback
    {
    }

    [Register("org/jitsi/meet/sdk/JitsiMeetActivityInterface", DoNotGenerateAcw = true)]
    internal class IJitsiMeetActivityInterfaceInvoker : Java.Lang.Object, IJitsiMeetActivityInterface
    {
        public IJitsiMeetActivityInterfaceInvoker(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer) { }
        public int CheckPermission(string permission, int pid, int uid) => 0;
        public int CheckSelfPermission(string permission) => 0;
        public bool ShouldShowRequestPermissionRationale(string permission) => false;
        public void RequestPermissions(string[] permissions, int requestCode, Com.Facebook.React.Modules.Core.IPermissionListener listener) { }
        public void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults) { }
    }
}
