

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
        public int frame;

        public bool isOpened;
        public bool isMoving;

        public void Open()
        {
            Vector3 destination = Direction.normalized * (moveScale);
            destination.Scale(transform.localScale);
            StartCoroutine(Move(transform.position+destination,time,frame));
            isOpened = true;
        }

        public void Close()
        {
            Vector3 destination = Direction.normalized * (moveScale);
            destination.Scale(transform.localScale);
            StartCoroutine(Move(transform.position-destination,time,frame));
            isOpened = false;
        }

        private IEnumerator Move(Vector3 destination, float time, int count = 16)
        {
            var delay = new WaitForSeconds(time/count);
            Vector3 deltaPos = destination - transform.position;
            isMoving = true;
            for (int i = 0; i < 16; i++)
            {
                transform.Translate(deltaPos/count);
                yield return delay;
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