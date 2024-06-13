namespace Treasure.Puzzle
{
    using UnityEngine;
    using Treasure.Common;
    using System.Collections.Generic;

    public class PuzzleSlotsGenerator : MonoBehaviour
    {
        [SerializeField] private LetterSlot _letterSlotTemplate;
        [SerializeField] private DropSlot _dropSlotTemplate;
        [Header("Configurations")]
        [SerializeField] private float _slotsSize = 60;
        [SerializeField] private float _slotsSpacing = 160;
        [SerializeField] private Vector2 _minLockPanelSize = new Vector2(550, 50);
        [SerializeField] private RectTransform _slotsParent;
        [SerializeField] private RectTransform _lockPanel;

        public DropSlot[] GenerateLetterSlots(WordData puzzleWord)
        {
            List<DropSlot> dropSlots = new List<DropSlot>();

            for (int i = 0; i < _slotsParent.childCount; i++)
            {
                Destroy(_slotsParent.GetChild(i).gameObject);
            }

            Vector2 panelSize = new Vector2(_slotsSize * puzzleWord.Word.Length + _slotsSpacing, _lockPanel.sizeDelta.y);

            int letterPosition = 0;
            foreach (var letter in puzzleWord.Word)
            {
                letterPosition++;
                if (puzzleWord.Word.Length >= 9)
                    panelSize = PanelSizeAdjuster.AdjustedPanelSize(panelSize, _minLockPanelSize);

                if (!ShouldGenerateDropSlot(letter, letterPosition, puzzleWord))
                {
                    LetterSlot letterSlot = Object.Instantiate(_letterSlotTemplate, _slotsParent);
                    letterSlot.Init(letter.ToString());
                    letterSlot.gameObject.name = $"{letter} Slot";
                    continue;
                }

                DropSlot dropSlot = Object.Instantiate(_dropSlotTemplate, _slotsParent);
                dropSlot.Init(letter.ToString());
                dropSlot.gameObject.name = $"{letter} Slot";
                dropSlots.Add(dropSlot);
            }

            _lockPanel.sizeDelta = panelSize;
            return dropSlots.ToArray();
        }

        private bool ShouldGenerateDropSlot(char letter, int letterPosition, WordData puzzleWord)
        {
            foreach (var missingLetter in puzzleWord.MissingLetters)
            {
                if (letter != missingLetter.letter) continue;
                if (letterPosition == missingLetter.position) return true;
            }
            return false;
        }
    }

}
