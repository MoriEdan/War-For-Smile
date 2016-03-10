using UnityEngine;
using System.Collections;

namespace Helpers
{
    public static class UserInput
    {
        public static Vector3 GetMovementDirection()
        {
            var direction = Vector3.zero;
            //direction.y *= -1;  // invert the y-axis

            if (Input.GetKey(KeyCode.D))
            {
                direction.x += 1;
            }

            if (Input.GetKey(KeyCode.A))
            {
                direction.x -= 1;
            }

            if (Input.GetKey(KeyCode.W))
            {
                direction.y += 1;

            }

            if (Input.GetKey(KeyCode.S))
            {
                direction.y -= 1;
            }

            // Clamp the length of the vector to a maximum of 1.
            if (direction.sqrMagnitude > 1)
            {
                direction.Normalize();
            }

            return direction;
        }

        public static Vector3 GetAimDirection(Vector3 playerPosition)
        {
            var direction = Input.mousePosition - playerPosition;

            if (direction == Vector3.zero)
            {
                return Vector3.zero;
            }
            else
            {
                return Vector3.Normalize(direction);
            }
        }
    }
}
