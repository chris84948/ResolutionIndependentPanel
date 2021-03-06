# Resolution Independent Panel

WPF claims to be resolution independent, and sometimes that's true. If you have an application that has a small UI, or has a lot of space on screen, you can sometimes get away with designing your UI so it will adjust nicely without drawing on top of each other.

Sometimes you have to create a complex UI and when designing it, you need every pixel of that 1920 x 1080 resolution. Then someone comes along and tells you it has to work on a 1280 x 800 old square monitor. Now, instead of trying to change your UI, and making it look worse on higher resolutions just for the sake of that one old monitor, you can use ResolutionIndependentPanel!

This is a panel you can add to your project that will automatically scale all the content within it based on the resolution of your screen containing it.

There are 2 dependency properties that must be used - `DesignWidth` and `DesignHeight`. These should be the resolution of the window on which you designed it. If you've made your complex UI on a 1080p monitor, use `DesignWidth="1920" DesignHeight="1080"`. ResolutionIndependentPanel will then check for the resolution of the screen it's on at startup and scale the content appropriately.

    ...
    xmlns:rip="clr-namespace:ResolutionIndependentPanel;assembly=ResolutionIndependentPanel"
    ...
    
    <rip:ResolutionIndependentPanel DesignHeight="1080"
                                    DesignWidth="1920">
        <Grid>{ Your Content }</Grid>
    </rip:ResolutionIndependentPanel>

Using this panel, you can get scaling for free. Here's an example (P.S. you might have to actually click and load the images to see the differences) -

UI Comparison - 1920x1080 vs 800x600
![](https://raw.githubusercontent.com/chris84948/ResolutionIndependentPanel/master/Images/panel%20comparison.png)

As designed resolution - 1920 x 1080
![](https://raw.githubusercontent.com/chris84948/ResolutionIndependentPanel/master/Images/Screenshot.1920x1080.png)

Scaled down resolution - 800 x 600
![](https://raw.githubusercontent.com/chris84948/ResolutionIndependentPanel/master/Images/Screenshot.800x600.png)
