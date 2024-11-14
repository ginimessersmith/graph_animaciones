using System.Collections.Generic;

namespace Graphic3D.Models
{
    public class Animation
    {

        public List<Action> Actions { get; private set; } = new List<Action>();
         
        private int _currentActionIndex;

        public Animation()
        {
            Actions = new List<Action>();
            _currentActionIndex = 0;
            
        }


        public void AddAction(Action action)
        {
            Actions.Add(action);
        }

        public void Update(float deltaTime)
        {
            if (_currentActionIndex < Actions.Count)
            {
                Action currentAction = Actions[_currentActionIndex];
                currentAction.Update(deltaTime);
                if (currentAction.IsCompleted)
                {
                    _currentActionIndex++;
                }
            }
        }

        public bool IsCompleted()
        {
            return _currentActionIndex >= Actions.Count;
        }

        public void Reset()
        {
            _currentActionIndex = 0;
            foreach (var action in Actions)
            {
                action.Reset();
            }
        }
    }
}