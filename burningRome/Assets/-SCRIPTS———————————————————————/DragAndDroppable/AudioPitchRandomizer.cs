using UnityEngine;
using GD.MinMaxSlider;
using NUnit.Framework;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class AudioPitchRandomizer : MonoBehaviour
{
    [SerializeField] List<AudioClip> sons;
    [SerializeField][MinMaxSlider(0.2f,5f)]Vector2 pitch = Vector2.one;

    void Start()
    {
        GetComponent<AudioSource>().pitch = Random.Range(pitch.x, pitch.y);
        //int index = Random.Range(0, sons.Count);
        //AudioClip sonchoisi = sons[index];
        //GetComponent<AudioSource>().PlayOneShot(sonchoisi);
    }
} // FIN DU SCRIPT

namespace GD.MinMaxSlider
{
    using UnityEngine;

    public class MinMaxSliderAttribute : PropertyAttribute{

        public float min;
        public float max;

        public MinMaxSliderAttribute(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }  
}
