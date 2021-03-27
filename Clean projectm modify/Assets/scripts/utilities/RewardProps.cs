using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class RewardProps
{
    public RewardItem[] content;
}

[Serializable]
public class RewardItem {
    public string RewardName;
    public string RotationAxis;
    public float RotationSpeed;
    public bool Reflect = false; 
}