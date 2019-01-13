# What is this?

These are a few sandbox experiments showing how to draw pixel-by-pixel in .NET.  This is not how modern frameworks want to do things, but if you're playing around with some old 80s/90s game rendering techniques it can be quite handy.  Graphics cards don't want to work like this either so this is pretty far from the optimal way of making a high-performance game, but it should be good enough for playing around.

## WpfCompositionTarget

Uses WPF's [<code>CompositionTarget.Rendering</code>](https://docs.microsoft.com/en-us/dotnet/api/system.windows.media.compositiontarget.rendering) event to draw to a [<code>WriteableBitmap</code>](https://docs.microsoft.com/en-us/dotnet/api/system.windows.media.imaging.writeablebitmap).  Also uses the [WriteableBitmapEx](https://github.com/teichgraf/WriteableBitmapEx/) library for convenience.