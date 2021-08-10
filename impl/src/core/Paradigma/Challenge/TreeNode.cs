using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Works.Paradigma.Challenge.Extensions;
using Works.Paradigma.Challenge.Helpers;

namespace Works.Paradigma.Challenge
{

	//Cria uma arvore binaria
    public class TreeNode
	{
		
	    private List<Node> nodeList = new List<Node>();

		public TreeNode()
		{
			IDGeneratorHelper.Instance.Reset(); //reseta o id 
		}
		public bool Sort { get; set; } = true;
		public bool IsRoot(Node node)
		{
			var result = node.ValuesEqual(GetRoot());
			return result;
		}

		#region add node
		//recebe um array bi-dimensional ex.: { { "A", "B" }, { "A", "C" } }. sortea se for necessario e o add na lista.
		public void Add(object[,] options)
		{

			object[,] arrValues = options;

			if (Sort) arrValues = options.OrderBy(x => x[0]);
			for (int i = 0; i < arrValues.GetLength(0); i++)
			{
				var parent = arrValues[i, 0];
				var children = arrValues[i, 1];
				Add(new object[] { parent, children });
			}
		}
		//recebe um array como argumento ex.: {"A","B"} e a inclui na lista
		public void Add(object[] value)
		{

			if (!value.GetType().IsArray)
			{
				AppHelpers.FireException(EnumTypeException.E4, "Argumento invalido! informe uma matriz bidimensional como argumento, ex: [A,B]");
			}
			var source = value[0];
			var child = value[1];
			if (source.ToString().Equals(child.ToString()))
			{
				AppHelpers.FireException(EnumTypeException.E4, "Objeto invalido! informe pai/filho como objetos diferentes");
			}

			var parent = new Node(source);
			var nodeChild = new Node(child);

			parent = this.AddParent(parent);
			this.AddChildren(parent, nodeChild);

		}
		//verifica se o node em questão ja existe como ancestral ou o recupera da lista.
		private Node AddParent(Node node)
		{
			var parent = GetNode(node);
			if (parent == null)
			{
				var root = GetRoot();
				if (root != parent)
				{

					if (!Sort)
					{
						return GetLastParent();
					}
					else
					{
						var message = $"Valor {node.Text} Inválido! Já existe um objeto raiz!";
						AppHelpers.FireException(EnumTypeException.E4, message);
					}

				}
				node.SetPosition(EnumPosition.Root);
				nodeList.Add(node);
				parent = node;
			}
			return parent;

		}
		//adiciona o node como children, verifica primeiro se ja existe como left, caso contrario, como right
		//ou dispara um erro caso o node parent ja esta populado os dois lados.
		private void AddChildren(Node parent, Node children)
		{
			//verificar se o valor ja existe na lista
			if (nodeList.Any(f => f.ValuesEqual(children)))
			{
				AppHelpers.FireException(EnumTypeException.E3);
			}
			//verificar se o valor existe como left
			var node = GetChildrens(parent).SingleOrDefault(f => f.Position == EnumPosition.Left);
			var position = EnumPosition.Left;
			if (node != null)
			{
				position = EnumPosition.Right;
				//verificar se o valor existe como right		   
				node = GetChildrens(parent).SingleOrDefault(f => f.Position == EnumPosition.Right);
				if (node != null)
				{
					AppHelpers.FireException(EnumTypeException.E1, parent.Text);
				}
			}
			children.SetParent(parent, position);
			nodeList.Add(children);

		}

        #endregion

        #region get node
		public List<Node> GetNodes()
        {
			return nodeList.OrderBy(f => f.Id).ToList();

		}
       
		private Node GetLastParent()
		{
			Node nodeSelected = null;
			foreach (var node in nodeList.Where(f => f.Parent != null))
			{
				var count = GetChildrens(node).Count();
				if (count < 2)
				{
					nodeSelected = node;
					break;
				}
			}
			return nodeSelected;
		}
		public Node GetRoot()
		{
			var nodeSelected = nodeList.SingleOrDefault(f => f.Parent == null);
			return nodeSelected;
		}
		private Node GetNode(Node node)
		{
			var nodeSelected = nodeList.SingleOrDefault(f => f.ValuesEqual(node));
			return nodeSelected;
		}	
		public Node GetAncestral(Node node)
        {
			var ancestral = node.Parent;
			return GetNode(ancestral);
        }
		public IEnumerable<Node> GetChildrens(Node root)
		{
			if (root == null) yield return null;
			var key = root?.Id;
			var lookup = nodeList.ToLookup(i => i.Parent?.Id);
			Stack<Node> st = new Stack<Node>(lookup[key]);

			while (st.Count > 0)
			{
				var item = st.Pop();
				yield return item;

			}
		}
		#endregion

		#region print
		
		private const string _cross = " ├─";
		private const string _corner = " └─";
		private const string _vertical = " │ ";
		private const string _space = "   ";

		//desenha a arvore com recurso grafico limitado.
		public void Draw(bool withGrafics = true)
		{
			if (withGrafics) PrintNode(GetRoot(), "");
			else PrintNode(GetRoot());

		}

		//desenha a arvore como um conjunto
		//ex.:
			//A[
			//   [B[D][G]]
			//   [C[E[F]][H]]]
			//]
		void PrintNode(Node node, int level = 0)
		{
			if (node == null) return;
			var text = $"{node.Text}";
			if (IsRoot(node)) text += $"[";
			Write(text);
			level++;


			var childrens = GetChildrens(node).OrderBy(f => f.Id).ToList();
			var numberOfChildren = childrens.Count;



			for (var i = 0; i < numberOfChildren; i++)
			{


				var child = childrens[i];
				var isLast = (i == (numberOfChildren - 1));

				text = $"[";
				if (level == 1)
				{
					Write(Environment.NewLine);
					text = $"{AppHelpers.RepeatChar(4)}[";
				}

			 
				Write(text);
				if (GetChildrens(child).Count() > 0)
				{
					PrintNode(child, level);
				}
				else
				{
					text = $"{child.Text}";
					Write(text);
				}
				text = "]";				 
				Write(text);

			}
			//text = "]";
			if (IsRoot(node))
			{
				text = $"{Environment.NewLine}]";
				Write(text);

			}

		}


		//desenha a arvore como recurso grafico limitado
		//ex.:
		// A
		//├─B
		//│  ├─G
		//│  └─D
		//└─C
		//   ├─H
		//   └─E
		//      └─F
		void PrintNode(Node node, string indent)
		{
			var text = $"{node.Text}{Environment.NewLine}";
			Write(text);

			// Loop through the children recursively, passing in the
			// indent, and the isLast parameter
			var childrens = GetChildrens(node).OrderBy(f => f.Id).ToList();
			var numberOfChildren = childrens.Count;
			for (var i = 0; i < numberOfChildren; i++)
			{
				var child = childrens[i];
				var isLast = (i == (numberOfChildren - 1));
				PrintChildNode(child, indent, isLast);
			}
		}

		void PrintChildNode(Node node, string indent, bool isLast)
		{
			// Print the provided pipes/spaces indent
			  Write(indent);

			// Depending if this node is a last child, print the
			// corner or cross, and calculate the indent that will
			// be passed to its children
			if (isLast)
			{
				Write(_corner);
				indent += _space;
			}
			else
			{
				Write(_cross);
				indent += _vertical;
			}

			PrintNode(node, indent);
		}
		#endregion


		#region actions
		public Action<string> Write { get; set; }
		#endregion
	}


}







