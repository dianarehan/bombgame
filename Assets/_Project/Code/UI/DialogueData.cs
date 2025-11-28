using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string characterName;
    [TextArea(3, 10)]
    public string sentence;
    public float duration = 3f;
}

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Conversation")]
public class DialogueData : ScriptableObject
{
    public DialogueLine[] lines;
}