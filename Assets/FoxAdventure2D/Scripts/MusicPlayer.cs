using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance;

    public AudioClip[] playlist;
    private AudioSource audioSource;
    private int currentTrackIndex = 0;

    void Awake()
    {
        // Đảm bảo chỉ có một MusicPlayer tồn tại
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Không bị phá khi chuyển scene
        }
        else
        {
            Destroy(gameObject); // Hủy clone nếu đã có một instance tồn tại
            return;
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.3f;
        PlayNextTrack();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNextTrack();
        }
    }

    void PlayNextTrack()
    {
        if (playlist.Length == 0) return;

        // Chọn một bài nhạc ngẫu nhiên khác với bài hiện tại
        int nextIndex;
        do
        {
            nextIndex = Random.Range(0, playlist.Length);
        } while (playlist.Length > 1 && nextIndex == currentTrackIndex);

        currentTrackIndex = nextIndex;
        audioSource.clip = playlist[currentTrackIndex];
        audioSource.Play();
    }
}
