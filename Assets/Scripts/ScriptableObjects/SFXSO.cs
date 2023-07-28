using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class SFXSO : ScriptableObject
{
    public AudioClip[] chop;
    public AudioClip[] deliveryFail;
    public AudioClip[] deliverySuccess;
    public AudioClip[] walk;
    public AudioClip[] sprint;
    public AudioClip[] objectDrop;
    public AudioClip[] objectPickup;
    public AudioClip panSizzle;
    public AudioClip[] trash;
    public AudioClip[] warn;
}
