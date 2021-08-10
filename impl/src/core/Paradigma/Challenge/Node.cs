using System;
using System.ComponentModel;
using Works.Paradigma.Challenge.Helpers;

namespace Works.Paradigma.Challenge
{

	//classe que representa um node/folha de uma arvore.
	//Id   : Id automatico
	//Value: objeto que sera incluso no node, string ou numero
	//Text : descricao do no 
	//Parent: objeto ancestral
	//Position: posicao na arvore que o node ocupará!
    public class Node : IEquatable<Node>, IValuesEquatable
	{
		public Node()
		{

		}
		public Node(object value) : this()
		{
			this.Value = value;
			this.Text = value.ToString();
		}
		public long Id { get; set; }
		public object Value { get; set; }
		public string Text { get; set; }
		[Description("Attributo")]
		public Node Parent { get; set; }
		public EnumPosition Position { get; set; }
		public void SetPosition(EnumPosition position)
		{
			this.Id = IDGeneratorHelper.Instance.Next();
			this.Position = position;
		}
		public void SetParent(Node parent, EnumPosition position)
		{
			SetPosition(position);
			this.Parent = parent;

		}
		public override string ToString()
		{
			return Text;
		}

		#region ValuesEquatable

		public virtual bool ValuesEqual(Node node)
		{
			if (node == null)
			{
				return false;
			}

			var valuesEqual = Equals(node);
			return valuesEqual;
		}
		public virtual bool ValuesEqual(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			if (ReferenceEquals(this, obj))
			{
				return true;
			}
			if (GetType() != obj.GetType())
			{
				return false;
			}

			return ValuesEqual(obj as Node);
		}

		#endregion

		#region Methods override
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			if (ReferenceEquals(this, obj))
			{
				return true;
			}
			if (obj.GetType() != typeof(Node))
			{
				return false;
			}
			return Equals((Node)obj);
		}
		public bool Equals(Node other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}

			return
				Equals(other.Text, Text) && Equals(other.Value.ToString(), Value.ToString());
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
		public override int GetHashCode()
		{
			unchecked
			{
				var result = (Text != null ? Text.GetHashCode() : 0);
				result = (result * 397) ^ (Value != null ? Value.GetHashCode() : 0);
				return result;
			}
		}



		#region Operators

		/// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator ==(Node left, Node right)
		{
			return Equals(left, right);
		}

		/// <summary>
		/// Implements the operator !=.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator !=(Node left, Node right)
		{
			return !Equals(left, right);
		}

		#endregion
		#endregion






	}
}
