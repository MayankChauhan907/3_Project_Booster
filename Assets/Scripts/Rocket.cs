using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
#pragma warning disable 0649

    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] float _RotationThrustSpeed = 100f;
    [SerializeField] float _mainThrustSpeed = 100f;
    [SerializeField] float _levelLoadDeley = 2f;
    bool _isCollisonOn = false;


    [SerializeField] AudioClip _mainThrustSFX;
    [SerializeField] AudioClip _deathSFX;
    [SerializeField] AudioClip _winSFX;


    [SerializeField] ParticleSystem _mainThrustParticles;
    [SerializeField] ParticleSystem _deathParticles;
    [SerializeField] ParticleSystem _winParticles;

    bool _isTrasceding = false;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isTrasceding)
        {
            RespondToThrustInput();
            RespondToRotationInput();
        }
        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isTrasceding || _isCollisonOn) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //Do Nothing
                break;

            case "Finish":
                StartSuccessSequence();
                break;

            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        _isTrasceding = true;
        audioSource.Stop();
        audioSource.PlayOneShot(_winSFX);
        _winParticles.Play();
        Invoke("LoadNextScene", _levelLoadDeley);
    }

    private void StartDeathSequence()
    {
        _isTrasceding = true;
        audioSource.Stop();
        audioSource.PlayOneShot(_deathSFX);
        _deathParticles.Play();
        Invoke("LoadFirstScene", _levelLoadDeley);
    }

    private void LoadNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;
        if (nextIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextIndex = 0;
        }
        SceneManager.LoadScene(nextIndex);
    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            _mainThrustParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * _mainThrustSpeed * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(_mainThrustSFX);
        }
        if (_mainThrustParticles.isPlaying) { return; }
        _mainThrustParticles.Play();
    }

    private void RespondToRotationInput()
    {

        float RotateThisFrame = _RotationThrustSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            rigidBody.angularVelocity = Vector3.zero;
            transform.Rotate(Vector3.forward * RotateThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rigidBody.angularVelocity = Vector3.zero;
            transform.Rotate(-Vector3.forward * RotateThisFrame);
        }
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            _isCollisonOn = !_isCollisonOn;
        }
    }
}
