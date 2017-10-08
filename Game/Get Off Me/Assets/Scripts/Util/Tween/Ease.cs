using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tween
{
    public static class Ease
    {
        // Parameter definitions
        // t: current time
        // b: start value
        // c: change in value
        // d: duration
        public delegate float EaseDelegate(float t, float b, float c, float d);

        public static EaseDelegate Linear = (float t, float b, float c, float d) => {
            return c * t / d + b;
        };
    }
}