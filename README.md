KinoFeedback
============

Oldskool frame buffer feedback effect for Unity

![gif][gif1] ![gif][gif2]

How to use
----------

- Import [the package file][package] into your project
- Add Feedback script to a camera object
- Start the play mode. Tweak the perameters as you like.

The Feedback script transfers the content of the previous frame to the current
frame with the least priority (the maximum depth value). You can tweak color,
offset, scale and rotation applied in transferring.

License
-------

Copyright (C) 2016 Keijiro Takahashi

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

[gif1]: http://49.media.tumblr.com/ea2fa90708675da8d76d8f0a6037e0cb/tumblr_o321kkmYGe1qio469o2_400.gif
[gif2]: http://49.media.tumblr.com/b469f77d326410a3ae1d5806d115c128/tumblr_o30c8kCX151qio469o1_400.gif
[package]: https://github.com/keijiro/KinoFeedback/blob/master/KinoFeedback.unitypackage
