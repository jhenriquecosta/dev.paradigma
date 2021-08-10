# DEV TEST
Foi criado uma pequena aplicação em BLAZOR, utilizando **N-TIER ARCHITECTURE**. Faço uso de uma biblioteca de Helper **WORKS.FRAMEWORK** que
agrega inumeras funcionalidades.
* WORKS.PARADIGMA.CORE - Esta camada fornece funcionalidades compartilhadas entre todos os modulos. Nesta camada está a resposta do exemplo 1.
                         classe **[TreeNode](https://github.com/jhenriquecosta/dev.paradigma/blob/master/impl/src/core/Paradigma/Challenge/TreeNode.cs)**, responsavel por incluir,
                         sortear e imprimir nodes.
                         ex.:
                         var tree = new TreeNode();                         
                         var data1 = new object[,] { { "A", "B" }, { "A", "C" }, { "B", "G" }, { "C", "H" }, { "E", "F" }, { "B", "D" }, { "C", "E" } };
                         tree.Sort = true;
                         tree.Add(data1);
                         

                         tree.Draw();  //imprime arvore com recurso grafico 1
                         resultado:
                         	  A
		                      ├─B
		                      │  ├─G
		                      │  └─D
		                      └─C
		                        ├─H
		                        └─E
		                          └─F

                         tree.Draw(false) //imprime arvore modo simples
                         resultado:
                            A[
			                  [B[D][G]]
			                   [C[E[F]][H]]]
			                 ]
                         tree.PrintConsoleOut(); //imprime arvore com recurso grafico.
                         resultado:
                             A
                           ┌─└─┐
                           B   C
                          ┌└┐ ┌└┐
                          G D H E
                               ┌┘
                               F


                  