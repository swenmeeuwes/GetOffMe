using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationUtil {
    public static IEnumerator OnAnimationFinished(Animation animation, Action callback)
    {
        while (animation.isPlaying)
            yield return null;
        callback.Invoke();
    }
}
