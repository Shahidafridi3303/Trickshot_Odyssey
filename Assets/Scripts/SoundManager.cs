using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip moveSound;
    [SerializeField] private AudioClip notYourMoveSound;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip drawSound;
    [SerializeField] private AudioClip turnChangeSound;
    [SerializeField] private AudioClip chatNotificationSound;
    [SerializeField] private AudioClip laserSound;
    [SerializeField] private AudioClip platformChangeSound;
    [SerializeField] private AudioClip freezeSound;
    [SerializeField] private AudioClip boxFallSound;
    [SerializeField] private AudioClip balloonMoveSound;
    [SerializeField] private AudioClip cameraShakeSound;
    [SerializeField] private AudioClip treasureActivateSound;
    [SerializeField] private AudioClip spikeHitSound;
    [SerializeField] private AudioClip sliderChangeSound;
    [SerializeField] private AudioClip bombExplodeSound;
    [SerializeField] private AudioClip windActivateSound;
    [SerializeField] private AudioClip windDeactivateSound;
    [SerializeField] private AudioClip BallBoxCollideSound;

    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void PlayButtonClick() => PlaySound(buttonClickSound);
    public void PlayMoveSound() => PlaySound(moveSound);
    public void PlayNotYourMoveSound() => PlaySound(notYourMoveSound);
    public void PlayWinSound() => PlaySound(winSound);
    public void PlayDrawSound() => PlaySound(drawSound);
    public void PlayTurnChangeSound() => PlaySound(turnChangeSound);
    public void PlayChatNotificationSound() => PlaySound(chatNotificationSound);
    public void PlayLaserSound() => PlaySound(laserSound);
    public void PlayPlatformChangeSound() => PlaySound(platformChangeSound);
    public void PlayFreezeSound() => PlaySound(freezeSound);
    public void PlayBoxFallSound() => PlaySound(boxFallSound);
    public void PlayBalloonMoveSound() => PlaySound(balloonMoveSound);
    public void PlayCameraShakeSound() => PlaySound(cameraShakeSound);
    public void PlayTreasureActivateSound() => PlaySound(treasureActivateSound);
    public void PlaySpikeHitSound() => PlaySound(spikeHitSound);
    public void PlaySliderChangeSound() => PlaySound(sliderChangeSound);
    public void PlayBombExplodeSound() => PlaySound(bombExplodeSound);
    public void PlayWindActivateSound() => PlaySound(windActivateSound);
    public void PlayWindDeactivateSound() => PlaySound(windDeactivateSound);
    public void PlayBallBoxCollideSound() => PlaySound(BallBoxCollideSound);
}
