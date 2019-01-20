# What is this?

These are a few sandbox experiments showing how to draw to the screen pixel-by-pixel in .NET.  This is not how modern frameworks want to do things, but if you're playing around with some old 80s/90s game rendering techniques it can be quite handy.  Graphics cards don't want to work like this either so this is pretty far from the optimal way of making a high-performance game, but it should be good enough for playing around.

Both of these examples can be improved upon if you remove the abstraction that allows the graphics backend to be swapped.

## WpfCompositionTarget

Uses WPF's [<code>CompositionTarget.Rendering</code>](https://docs.microsoft.com/en-us/dotnet/api/system.windows.media.compositiontarget.rendering) event to draw to a [<code>WriteableBitmap</code>](https://docs.microsoft.com/en-us/dotnet/api/system.windows.media.imaging.writeablebitmap).  Also uses the [WriteableBitmapEx](https://github.com/teichgraf/WriteableBitmapEx/) library for convenience.

## MonoGameTexture

Uses [MonoGame](http://www.monogame.net/) to draw to a texture the same size as the screen.  Written with MonoGame 3.7.1 if it matters.