
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openSpeed = 2f;
    [SerializeField] private float closeSpeed = 4f;
    [SerializeField] private bool isOpen = false;

    [Header("Door Sounds")]
    [SerializeField] private AudioSource doorAudioSource;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;

    private Quaternion _closedRotation;
    private Quaternion _openRotation;
    private Coroutine _currentCoroutine;

    void Start()
    {
        _closedRotation = transform.rotation;

        _openRotation = Quaternion.Euler(
            transform.eulerAngles + new Vector3(0, openAngle, 0)
        );
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);

            _currentCoroutine = StartCoroutine(ToggleDoor());
        }
    }

    private IEnumerator ToggleDoor()
    {
        bool opening = !isOpen;

        Quaternion targetRotation = opening ? _openRotation : _closedRotation;

        float currentSpeed = opening ? openSpeed : closeSpeed;

        isOpen = opening;

        if (doorAudioSource != null)
        {
            AudioClip sound = opening ? openSound : closeSound;

            if (sound != null)
                doorAudioSource.PlayOneShot(sound);
        }

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
        {
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * currentSpeed
            );

            yield return null;
        }

        transform.rotation = targetRotation;
        _currentCoroutine = null;
    }
}
