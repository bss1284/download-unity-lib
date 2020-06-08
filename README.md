# Unity Download Plugin

## Feature
- Downloaded from Android application background.
- Send a Notification when the download is complete.
- Source code included.

## Classes
- __NativeManager  (Assets/Project/Scripts/NativeManager.cs)__   
Wrapping class for Android native plugin.  
Unity calls native code through this class.  

- __PluginManager  (Assets/Plugins/Android/PluginManager.kt)__  
Android native plugin class. (Kotlin)  
It performs functions such as Notification Send, Download Data using http url.

## HOW TO
1. Click download button.
2. Go to application background. (Please press home button)
3. Wait when download is complete.
4. Click complete notification.
5. The background has been changed to the downloaded file.

## Note
__This plugin has been tested in the specifications below.__  
- Unity Version : "2019.3.3f"  
- Android Model : "Samsung Galaxy 7"  
- Android Min SDK Version : 19  
- Android Compile SDK Version : 28  
- Android Build Tools Version : "28.0.3"  

## Dependencies
- org.jetbrains.kotlin:kotlin-stdlib-jdk7:1.3.50  
- androidx.appcompat:appcompat:1.1.0  
- androidx.core:core-ktx:1.2.0  
- unity-jar-resolver 
 (https://github.com/googlesamples/unity-jar-resolver)

### CREATE BY
- Sanghun Lee
- Email: tkdgns1284@gmail.com
- Github: https://github.com/bss1284