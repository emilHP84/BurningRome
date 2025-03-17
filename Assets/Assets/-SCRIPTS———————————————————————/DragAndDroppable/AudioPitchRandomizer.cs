using UnityEngine;
using GD.MinMaxSlider;

[RequireComponent(typeof(AudioSource))]
public class AudioPitchRandomizer : MonoBehaviour
{
    [SerializeField][MinMaxSlider(0.2f,5f)]Vector2 pitch = Vector2.one;

    void Start()
    {
        GetComponent<AudioSource>().pitch = Random.Range(pitch.x, pitch.y);
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
