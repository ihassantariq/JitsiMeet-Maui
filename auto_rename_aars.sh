#!/bin/bash
while true; do
  echo "Building..."
  dotnet build JitsiMeetDemo/JitsiMeetDemo.csproj -f net9.0-android -v:n > demo_build_output.txt 2>&1
  if [ $? -eq 0 ]; then
    echo "Build succeeded!"
    break
  fi
  
  # Find the conflicting aar name from the log
  AAR_NAME=$(grep -o "is from '[^']*.aar'" demo_build_output.txt | head -n 1 | cut -d"'" -f2)
  if [ -z "$AAR_NAME" ]; then
    echo "Build failed for another reason. Check demo_build_output.txt"
    break
  fi
  
  echo "Found conflict: $AAR_NAME"
  
  # Find the aar in NativeBinaries/android/deps and rename it
  AAR_PATH=$(find NativeBinaries/android/deps -name "$AAR_NAME")
  if [ -z "$AAR_PATH" ]; then
    echo "Could not find $AAR_NAME in NativeBinaries/android/deps"
    break
  fi
  
  echo "Renaming $AAR_PATH to ${AAR_PATH%.aar}.zip"
  mv "$AAR_PATH" "${AAR_PATH%.aar}.zip"
done
