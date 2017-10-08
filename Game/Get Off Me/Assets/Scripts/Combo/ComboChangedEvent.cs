using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboChangedEvent {
    public int OldCombo { get; set; }
    public int NewCombo { get; set; }
    public int ComboDelta {
        get {
            return NewCombo - OldCombo;
        }
    }
}
