using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Vial Context")]
public class VialContext : ScriptableObject {
    public List<VialData> data = new List<VialData>();
}
