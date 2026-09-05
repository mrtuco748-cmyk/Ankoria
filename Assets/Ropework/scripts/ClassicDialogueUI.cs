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
            if (optionButtons != null) {
                foreach (var button in optionButtons) {
                    if (button != null) button.gameObject.SetActive (false);
                }
            }
            if (continuePrompt != null)
                continuePrompt.SetActive (false);
        }

        public override async YarnTask RunLineAsync(LocalizedLine line, LineCancellationToken token)
        {
            if (lineText == null) { Debug.LogError("ClassicDialogueUI lineText not assigned"); return; }
            lineText.gameObject.SetActive (true);

            string speakerName = "";
            string lineTextDisplay = line.Text.Text;
            if ( line.Text.Text.Contains(":") ) {
                var splitLine = line.Text.Text.Split( new char[] {':'}, 2);
                speakerName = splitLine[0].Trim();
                lineTextDisplay = splitLine[1].Trim();
            }
            
            if (nameText != null) {
                if ( speakerName.Length > 0 ) {
                    if (nameText.transform.parent != null) nameText.transform.parent.gameObject.SetActive(true);
                    nameText.text = speakerName;
                    if ( ropework != null && ropework.actorColors.ContainsKey(speakerName) ) {
                        var img = nameText.transform.parent.GetComponent<Image>();
                        if (img != null) img.color = ropework.actorColors[speakerName];
                    }
                    if ( ropework != null && ropework.actors.ContainsKey(speakerName) ) {
                        ropework.HighlightSprite( ropework.actors[speakerName] );
                    }
                } else {
                    if (nameText.transform.parent != null) nameText.transform.parent.gameObject.SetActive(false);
                }
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
            await YarnTask.Yield();

            if (continuePrompt != null)
                continuePrompt.SetActive (false);
        }

        public override async YarnTask<DialogueOption> RunOptionsAsync(DialogueOption[] dialogueOptions, LineCancellationToken cancellationToken)
        {
            var ct = cancellationToken.NextContentToken;
            if (optionButtons == null || optionButtons.Count == 0) {
                Debug.LogError("ClassicDialogueUI optionButtons not assigned");
                await YarnTask.Yield();
                return dialogueOptions.Length > 0 ? dialogueOptions[0] : null;
            }
            if (dialogueOptions.Length > optionButtons.Count) {
                Debug.LogWarning("There are more options to present than there are buttons.");
            }

            _optionSelected = false;
            _selectedOption = null;

            int i = 0;
            foreach (var option in dialogueOptions) {
                if (i >= optionButtons.Count) break;
                if (optionButtons[i] == null) { i++; continue; }
                optionButtons[i].gameObject.SetActive (true);
                var txt = optionButtons[i].GetComponentInChildren<Text>();
                if (txt != null) txt.text = option.Line.Text.Text;
                var opt = option;
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => SetOption(opt));
                i++;
            }

            while (!_optionSelected && !ct.IsCancellationRequested) {
                await YarnTask.Yield();
            }

            foreach (var button in optionButtons) {
                if (button == null) continue;
                button.gameObject.SetActive (false);
                button.onClick.RemoveAllListeners();
            }

            if (ct.IsCancellationRequested) return null;
            return _selectedOption;
        }

        public void SetOption(DialogueOption selectedOption)
        {
            _selectedOption = selectedOption;
            _optionSelected = true;
        }

        public void SetOption(int selectedOptionIndex)
        {
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
