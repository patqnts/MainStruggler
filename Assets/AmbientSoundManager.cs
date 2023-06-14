using UnityEngine;

public class AmbientSoundManager : MonoBehaviour
{
    public AudioSource[] ambientList;
    public DayNightCycles dayNightCycles;

    private bool isNight;

    private void Start()
    {
        dayNightCycles = FindObjectOfType<DayNightCycles>();
    }

    private void Update()
    {
        if (dayNightCycles != null)
        {
            isNight = dayNightCycles.isNight;

            if (isNight)
            {
                PlayAmbientSound(1);
                StopAmbientSound(0);
            }
            else
            {
                PlayAmbientSound(0);
                StopAmbientSound(1);
            }
        }
    }

    private void PlayAmbientSound(int index)
    {
        if (index >= 0 && index < ambientList.Length)
        {
            if (!ambientList[index].isPlaying)
            {
                ambientList[index].Play();
            }
        }
    }

    private void StopAmbientSound(int index)
    {
        if (index >= 0 && index < ambientList.Length)
        {
            if (ambientList[index].isPlaying)
            {
                ambientList[index].Stop();
            }
        }
    }
}
