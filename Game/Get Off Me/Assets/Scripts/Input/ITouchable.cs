using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITouchable {
    HashSet<int> FingerIds { get; set; }

    void OnTouchBegan(Touch touch);
    void OnTouch(Touch touch);
    void OnTouchEnded(Touch touch);
}
