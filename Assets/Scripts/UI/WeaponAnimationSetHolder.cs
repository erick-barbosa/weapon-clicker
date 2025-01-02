using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationSetHolder : MonoBehaviour {
    public List<Sprite> states = new List<Sprite>(3);
    public List<AnimationSet> animations = new List<AnimationSet>();
    public List<AudioClip> attackSounds = new List<AudioClip>(3);
}
