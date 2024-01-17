

using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Scripts.Game.Dungeon.Unit
{
    public class MoveDoor : BaseInteractionUnit,IDoor, IButtonListener
    {
        public Vector3 Direction;
        public float moveScale;
        public float time;

        public bool isOpened;
        public bool isMoving;

        public void Open()
        {
            Vector3 destination = Direction.normalized * (moveScale);
            destination.Scale(transform.localScale);
            StartCoroutine(Move(transform.position+destination,time));
            isOpened = true;
        }

        public void Close()
        {
            Vector3 destination = Direction.normalized * (moveScale);
            destination.Scale(transform.localScale);
            StartCoroutine(Move(transform.position-destination,time));
            isOpened = false;
        }

        private IEnumerator Move(Vector3 destination, float time)
        {
            float frameAmount = time/Time.fixedDeltaTime;
            Vector3 deltaPos = destination - transform.position;
            isMoving = true;
            for (int i = 0; i < frameAmount; i++)
            {
                transform.Translate(deltaPos/frameAmount);
                yield return new WaitForFixedUpdate();
            }

            isMoving = false;
            transform.position = destination;
        }

        public void Do()
        {
            if(!isOpened)
                Open();
        }

        public void Toggle()
        {
            if (isMoving)
                return;
            if (isOpened)
                Close();
            else
                Open();
        }
    }
}