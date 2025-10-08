using Common.Domain;
using Common.Enums;
using System.Text.Json.Serialization;

namespace Core.Domain
{
    public class TransportDuration : ValueObject<TransportDuration>
    {
        public int Time { get; private set; }
        public TransportType TransportType { get; private set; }

        [JsonConstructor]
        public TransportDuration(int time, TransportType transportType)
        {
            this.Time = time;
            this.TransportType = transportType;
        }

        protected override bool EqualsCore(TransportDuration other)
        {
            return Time == other.Time &&
                    TransportType == other.TransportType;
        }

        protected override int GetHashCodeCore()
        {
            unchecked
            {
                int hashCode = Time.GetHashCode();
                hashCode = (hashCode * 397) ^ TransportType.GetHashCode();

                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"{TransportType} ({Time} minutes)";
        }

    }
}
