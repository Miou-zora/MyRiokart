using UnityEngine;
using System;
using Unity.Netcode;

namespace KartGame.KartSystems
{
    [System.Serializable]
    public class InputData : IEquatable<InputData>, INetworkSerializable
    {
        public bool Accelerate;
        public bool Brake;
        public float TurnInput;
        public bool Item;


        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Accelerate);
            serializer.SerializeValue(ref Brake);
            serializer.SerializeValue(ref TurnInput);
            serializer.SerializeValue(ref Item);
        }

        public bool Equals(InputData other) 
        {
            if (other == null) 
                return false;

            return Accelerate == other.Accelerate && Brake == other.Brake && TurnInput == other.TurnInput && Item == other.Item;
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + Accelerate.GetHashCode();
                hash = hash * 23 + Brake.GetHashCode();
                hash = hash * 23 + TurnInput.GetHashCode();
                hash = hash * 23 + Item.GetHashCode();
                return hash;
            }
        }

        // equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            InputData other = (InputData)obj;
            return Accelerate == other.Accelerate && Brake == other.Brake && TurnInput == other.TurnInput && Item == other.Item;
        }
    }

    public interface IInput
    {
        InputData GenerateInput();
    }

    public abstract class BaseInput : MonoBehaviour, IInput
    {
        /// <summary>
        /// Override this function to generate an XY input that can be used to steer and control the car.
        /// </summary>
        public abstract InputData GenerateInput();
    }
}
