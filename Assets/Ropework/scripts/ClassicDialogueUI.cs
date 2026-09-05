using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using Yarn.Unity;

namespace Yarn.Unity.Example {
    public class ClassicDialogueUI : DialoguePresenterBase
    {
        public Ropework.RopeworkManager ropework;
        public Text nameText;
        public GameObject dialogueContainer;
        public Text lineText;
        public GameObject continuePrompt;
        public float textSpeed = 0.025f;
        public List<Button> optionButtons;
        public RectTransform gameControlsContainer;

        private DialogueOption _selectedOption;
        private bool _optionSelected;

        void Awake ()
        {
            if ( ropework == null ) { ropework = FindFirstObjectByType<Ropework.RopeworkManager>(); }
            if (dialogueContainer != null)
                dialogueContainer.SetActive(false);
            if (lineText != null)
                lineText.gameObject.SetActive (false);
            foreach (var button in optionButtons) {
                button.gameObject.SetActive (false);
            }
            if (continuePrompt != null)
                continuePrompt.SetActive (false);
        }

        public override async YarnTask RunLineAsync(LocalizedLine line, LineCancellationToken token)
        {
            lineText.gameObject.SetActive (true);

            string speakerName = "";
            string lineTextDisplay = line.Text.Text;
            if ( line.Text.Text.Contains(":") ) {
                var splitLine = line.Text.Text.Split( new char[] {':'}, 2);
                speakerName = splitLine[0].Trim();
                lineTextDisplay = splitLine[1].Trim();
            }
            
            if ( speakerName.Length > 0 ) {
                nameText.transform.parent.gameObject.SetActive(true);
                nameText.text = speakerName;
                if ( ropework != null && ropework.actorColors.ContainsKey(speakerName) ) {
                    nameText.transform.parent.GetComponent<Image>().color = ropework.actorColors[speakerName];
                }
                if ( ropework != null && ropework.actors.ContainsKey(speakerName) ) {
                    ropework.HighlightSprite( ropework.actors[speakerName] );
                }
            } else {
                nameText.transform.parent.gameObject.SetActive(false);
            }

            if (textSpeed > 0.0f) {
                var stringBuilder = new StringBuilder ();
                bool earlyOut = false;
                await YarnTask.Yield();
                foreach (char c in lineTextDisplay) {
                    if (token.NextContentToken.IsCancellationRequested) { earlyOut = true; break; }
                    float timeWaited = 0f;
                    stringBuilder.Append (c);
                    lineText.text = stringBuilder.ToString ();
                    while ( timeWaited < textSpeed ) {
                        timeWaited += Time.deltaTime;
                        if ( Input.anyKeyDown ) {
                            lineText.text = lineTextDisplay;
                            earlyOut = true;
                            break;
                        }
                    if (token.NextContentToken.IsCancellationRequested) { earlyOut = true; break; }
                        await YarnTask.Yield();
                    }
                    if ( earlyOut ) { break; }
                }
                if (!earlyOut) lineText.text = lineTextDisplay;
            } else {
                lineText.text = lineTextDisplay;
            }

            if (continuePrompt != null)
                continuePrompt.SetActive (true);

            while (!token.NextContentToken.IsCancellationRequested && !Input.anyKeyDown) {
                await YarnTask.Yield();
            }
            // wait one frame to consume the key press
            await YarnTask.Yield();

            if (continuePrompt != null)
                continuePrompt.SetActive (false);
        }

        [System.Obsolete]
        public override async YarnTask<DialogueOption> RunOptionsAsync(DialogueOption[] dialogueOptions, CancellationToken cancellationToken)
        {
            if (dialogueOptions.Length > optionButtons.Count) {
                Debug.LogWarning("There are more options to present than there are buttons.");
            }

            _optionSelected = false;
            _selectedOption = null;

            int i = 0;
            foreach (var option in dialogueOptions) {
                if (i >= optionButtons.Count) break;
                optionButtons[i].gameObject.SetActive (true);
                optionButtons[i].GetComponentInChildren<Text>().text = option.Line.Text.Text;
                // capture index
                int idx = i;
                var opt = option;
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => SetOption(opt));
                i++;
            }

            while (!_optionSelected && !cancellationToken.IsCancellationRequested) {
                await YarnTask.Yield();
            }

            foreach (var button in optionButtons) {
                button.gameObject.SetActive (false);
                button.onClick.RemoveAllListeners();
            }

            if (cancellationToken.IsCancellationRequested) return null;
            return _selectedOption;
        }

        // Overload with LineCancellationToken (Yarn 3.2)
        public override async YarnTask<DialogueOption> RunOptionsAsync(DialogueOption[] dialogueOptions, LineCancellationToken cancellationToken)
        {
#pragma warning disable CS0618
            return await RunOptionsAsync(dialogueOptions, cancellationToken.NextContentToken);
#pragma warning restore CS0618
        }

        public void SetOption(DialogueOption selectedOption)
        {
            _selectedOption = selectedOption;
            _optionSelected = true;
        }

        // Called by Unity UI Button with int index (legacy)
        public void SetOption(int selectedOptionIndex)
        {
            // fallback if wired via inspector with int
            Debug.LogWarning("SetOption(int) called - use DialogueOption overload. Index: " + selectedOptionIndex);
            _optionSelected = true;
        }

        public override YarnTask OnDialogueStartedAsync()
        {
            if (dialogueContainer != null)
                dialogueContainer.SetActive(true);
            if (gameControlsContainer != null) {
                gameControlsContainer.gameObject.SetActive(false);
            }
            return YarnTask.CompletedTask;
        }

        public override YarnTask OnDialogueCompleteAsync()
        {
            if (dialogueContainer != null)
                dialogueContainer.SetActive(false);
            if (gameControlsContainer != null) {
                gameControlsContainer.gameObject.SetActive(true);
            }
            return YarnTask.CompletedTask;
        }
    }
}
