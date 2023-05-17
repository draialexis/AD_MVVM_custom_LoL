using System;
namespace Model
{
	public class LargeImage : IEquatable<LargeImage>
	{
		public string Base64 { get; set; }

		public LargeImage(string base64)
		{
			Base64 = base64;
		}

        public bool Equals(LargeImage? other)
			=> other != null && other.Base64.Equals(Base64);

        public override bool Equals(object? obj)
        {
            if(ReferenceEquals(obj, null)) return false;
            if(ReferenceEquals(obj!, this)) return true;
            if(GetType() != obj!.GetType()) return false;
            return Equals(obj! as LargeImage);
        }

        public override int GetHashCode()
            => Base64.Substring(0, 10).GetHashCode();
    }
}

