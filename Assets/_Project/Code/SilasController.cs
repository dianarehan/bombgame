using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SphereCollider))]
public class EnemyDialogueController : MonoBehaviour
{
    [Header("Data")]
    public DialogueData conversation;
    [Tooltip("The single long audio file containing all the lines.")]
    public AudioClip fullConversationAudio; 
    [SerializeField ] private CinemachineCamera cam1;
    [SerializeField] private CinemachineCamera cam2;
    [Header("Settings")]
    public bool playOnTrigger = true;
    public bool playOnce = true;

    private AudioSource audioSource;
    private Animator animator;
    private bool hasPlayed = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        GetComponent<SphereCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playOnTrigger && !hasPlayed && other.CompareTag("Player"))
        {
            StartCoroutine(PlayDialogueRoutine());
            if (playOnce) hasPlayed = true;
        }
    }

    private IEnumerator PlayDialogueRoutine()
    {
        if (fullConversationAudio != null)
        {
            audioSource.clip = fullConversationAudio;
            audioSource.Play();
        }

        animator.SetBool("IsTalking", true);
        cam2.Priority = 10;
        cam1.Priority = 0;
        int styleIndex = 0; 

        foreach (DialogueLine line in conversation.lines)
        {
            animator.SetInteger("TalkStyle", styleIndex);
            styleIndex = (styleIndex == 0) ? 1 : 0;

            if (GameUIManager.Instance != null)
            {
                GameUIManager.Instance.ShowMessage(line.characterName + ": " + line.sentence);
            }


            yield return new WaitForSeconds(line.duration);
        }

        animator.SetBool("IsTalking", false);
        audioSource.Stop();
        cam2.Priority = 0;
        cam1.Priority = 10;

        if (GameUIManager.Instance != null)
        {
            GameUIManager.Instance.ShowMessage(""); 
        }
    }
}