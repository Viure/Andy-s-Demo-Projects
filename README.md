# Andy-s-Demo-Projects
These demos are for public consumption

CortanaDemo demostrates how you can add Cortana Speech recognition (for dictation in the demo) to a UWP project that is based on and inherites from a PCL forms app. <p>

Key Points <p>

1)  Create a factory delegate for the classes in which you want to make platform specific code.<p>
2)  In this case UWP project replaced the default delegate with one that creates Windows 
specific code.<p>
3)  The platform specific code inherits from the PCL ContentPage.  It gains access to the base 
classes controls via public accessor properties.  (I did not attempt to create a XAML page for 
the inherited properties.  Instead the base class creates the extra controls but leaves them 
invisible)<p>
4)  The speech recognition callbacks run in a diffent thread than the window and thus have to 
update it asynchornously.<p>

Presumably, you could use this same technique to add Android, or Siri based regognition, although i did not include that in the demo.  You'd be able to use the same extra controls.  In all platform specific overrides. <p>

There are some bugs in terms of error handling, but this should demonstrate the technique quite nicely.  Enjoy and please send comments.




      


