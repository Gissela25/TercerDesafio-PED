using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Threading;

namespace Tercer_Desafio_PED
{
    public partial class Form1 : Form
    {
        private CGrafo grafo; // instanciamos la clase CGrafo
        private CVertice nuevoNodo; // instanciamos la clase CVertice
        private CVertice NodoOrigen; // instanciamos la clase CVertice
        private CVertice NodoDestino; // instanciamos la clase CVertice
        private Recorrido rec;
        private int var_control = 0; // la utilizaremos para determinar el estado en la pizarra:
        // 0 -> sin acción, 1 -> Dibujando arco, 2 -> Nuevo vértice
        // variables para el control de ventanas modales
        private Eliminar eliminarnodo; //Ventana Eliminarcs para varios usos con los algoritmos y recorridos
        //private Recorrido ventanaRecorrido; // ventana para seleccionar el nodo inicial del recorrido
        private Label[] arreglo, arreglo2; //Arreglos de Label se usan para la simulacion de la cola, pila y vector
        private Vertice ventanaVertice; // ventana para agregar los vértices
        private Arco ventanaArco; // ventana para agregar los arcos
        List<CVertice> nodosRuta; // Lista de nodos utilizada para almacenar la ruta
        List<CVertice> nodosOrdenados; // Lista de nodos ordenadas a partir del nodo origen
        bool buscarRuta = false, nuevoVertice = false, nuevoArco = false;
        private int numeronodos = 0, opc; //Enteros para definir las diferentes opciones y el numero de nodos
        double peso = 0.0;
        bool profundidad = false, anchura = false, nodoEncontrado = false;

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex > -1)
            {
                foreach (CVertice nodo in grafo.nodos)
                {
                    foreach (CArco arco in nodo.ListaAdyacencia)
                    {
                        if ("(" + nodo.Valor + "," + arco.nDestino.Valor + ") peso: " + arco.peso ==
                       comboBox2.SelectedItem.ToString())
                        {
                            nodo.ListaAdyacencia.Remove(arco);
                            break;
                        }
                    }
                }
                nuevoVertice = true;
                nuevoArco = true;
                comboBox2.SelectedIndex = -1;
                Pizarra.Refresh();
            }
            else
            {
                label5.Text = "Seleccione un arco";
                label5.ForeColor = Color.Red;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox5.SelectedIndex > -1)
            {
                profundidad = true;
                origen = comboBox5.SelectedItem.ToString();
                Pizarra.Refresh();
                comboBox5.SelectedIndex = -1;
            }
            else
            {
                label5.Text = "Seleccione un nodo de partida";
                label5.ForeColor = Color.Red;

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox5.SelectedIndex > -1)
            {
                origen = comboBox5.SelectedItem.ToString();
                anchura = true;
                Pizarra.Refresh();
                comboBox5.SelectedIndex = -1;
            }
            else
            {
                label5.Text = "Seleccione un nodo de partida";
                label5.ForeColor = Color.Red;
            }
        }

        private List<List<int>> matrizFord = new List<List<int>>(); // matriz para utilizar algoritmo de ford
        private List<List<int>> matrizDistanciaWarshall = new List<List<int>>(); //matriz de distancias de cada nodo con cada nodo si existe relacion entre ellos
        private List<List<int>> matrizNodosWarshall = new List<List<int>>(); //matriz de nodos
        private Queue<int> Cola = new Queue<int>(); //cola de nodos

        private void button7_Click(object sender, EventArgs e)
        {
            //if (textBox1.Text.Trim() != "")
            //{
            //    if (grafo.BuscarVertice(textBox1.Text) != null)
            //    {
            //        label5.Text = "Si se encuentra el vértice " + textBox1.Text;
            //        label5.ForeColor = Color.Black;
            //    }
            //    else
            //    {
            //        label5.Text = "No se encuentra el vértice " + textBox1.Text;
            //        label5.ForeColor = Color.Red;
            //    }
            //}
            if (textBox1.Text.Trim() != "")
            {
                if (grafo.BuscarVertice(textBox1.Text) != null)
                {
                    MessageBox.Show("Si se encuentra el vértice " + textBox1.Text, "Buscar Vértice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No se encuentra el vértice " + textBox1.Text, "Buscar Vértice", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Pizarra_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                grafo.DibujarGrafo(e.Graphics);
                if (nuevoVertice)
                {
                    comboBox1.Items.Clear();
                    comboBox1.SelectedIndex = -1;
                    comboBox5.Items.Clear();
                    comboBox5.SelectedIndex = -1;
                    foreach (CVertice nodo in grafo.nodos)
                    {
                        comboBox1.Items.Add(nodo.Valor);
                        comboBox5.Items.Add(nodo.Valor);
                    }
                    nuevoVertice = false;
                }
                if (nuevoArco)
                {
                    comboBox2.Items.Clear();
                    comboBox2.SelectedIndex = -1;
                    foreach (CVertice nodo in grafo.nodos)
                    {
                        foreach (CArco arco in nodo.ListaAdyacencia)
                            comboBox2.Items.Add("(" + nodo.Valor + "," + arco.nDestino.Valor + ") peso: " + arco.peso);
                    }
                    nuevoArco = false;
                }
                if (buscarRuta)
                {
                    foreach (CVertice nodo in nodosRuta)
                    {
                        nodo.colorear(e.Graphics);
                        Thread.Sleep(1000);
                        nodo.DibujarVertice(e.Graphics);
                    }
                    buscarRuta = false;
                }
                if (profundidad)
                {
                    //ordenando los nodos desde el que indica el usuario
                    ordenarNodos();
                    foreach (CVertice nodo in nodosOrdenados)
                    {
                        if (!nodo.Visitado)
                            recorridoProfundidad(nodo, e.Graphics);
                    }
                    profundidad = false;
                    //reestablecer los valroes
                    foreach (CVertice nodo in grafo.nodos)
                        nodo.Visitado = false;

                }
                if (anchura)
                {
                    distancia = 0;
                    //ordenando los nodos desde el que indica el usuario
                    cola = new Queue();
                    ordenarNodos();
                    foreach (CVertice nodo in nodosOrdenados)
                    {
                        if (!nodo.Visitado && !nodoEncontrado)
                            recorridoAnchura(nodo, e.Graphics, destino);
                    }
                    anchura = false;
                    nodoEncontrado = false;
                    //reestablecer los valroes
                    foreach (CVertice nodo in grafo.nodos)
                        nodo.Visitado = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Pizarra_MouseLeave(object sender, EventArgs e)
        {
            Pizarra.Refresh();
        }

        private void Pizarra_MouseUp(object sender, MouseEventArgs e)
        {
            switch (var_control)
            {
                case 1: // Dibujando arco
                    if ((NodoDestino = grafo.DetectarPunto(e.Location)) != null && NodoOrigen !=
                   NodoDestino)
                    {
                        ventanaArco.Visible = false;
                        ventanaArco.control = false;
                        ventanaArco.ShowDialog();
                        if (ventanaArco.control)
                        {
                            if (grafo.AgregarArco(NodoOrigen, NodoDestino, ventanaArco.dato)) //Se procede a crear la arista
                            {
                                int distancia = ventanaArco.dato;
                                NodoOrigen.ListaAdyacencia.Find(v => v.nDestino == NodoDestino).peso =
                               distancia;
                            }
                            nuevoArco = true;
                        }
                    }
                    var_control = 0;
                    NodoOrigen = null;
                    NodoDestino = null;
                    Pizarra.Refresh();
                    break;
            }
        }

        private void Pizarra_MouseMove(object sender, MouseEventArgs e)
        {
            switch (var_control)
            {
                case 2: //Creando nuevo nodo
                    if (nuevoNodo != null)
                    {
                        int posX = e.Location.X;
                        int posY = e.Location.Y;
                        if (posX < nuevoNodo.Dimensiones.Width / 2)
                            posX = nuevoNodo.Dimensiones.Width / 2;
                        else if (posX > Pizarra.Size.Width - nuevoNodo.Dimensiones.Width / 2)
                            posX = Pizarra.Size.Width - nuevoNodo.Dimensiones.Width / 2;
                        if (posY < nuevoNodo.Dimensiones.Height / 2)
                            posY = nuevoNodo.Dimensiones.Height / 2;
                        else if (posY > Pizarra.Size.Height - nuevoNodo.Dimensiones.Width / 2)
                            posY = Pizarra.Size.Height - nuevoNodo.Dimensiones.Width / 2;
                        nuevoNodo.Posicion = new Point(posX, posY);
                        Pizarra.Refresh();
                        nuevoNodo.DibujarVertice(Pizarra.CreateGraphics());
                    }
                    break;

                case 1: // Dibujar arco



                    AdjustableArrowCap bigArrow = new AdjustableArrowCap(4, 4, true);
                    bigArrow.BaseCap = System.Drawing.Drawing2D.LineCap.Triangle;
                    Pizarra.Refresh();
                    Pizarra.CreateGraphics().DrawLine(new Pen(Brushes.Black, 2) { CustomEndCap = bigArrow },
                         NodoOrigen.Posicion, e.Location);
                    break;




            }
        }

        Queue cola = new Queue(); //para el recorrido de anchura
        private string destino = "", origen = "";
        private int distancia = 0;
        public Form1()
        {
            InitializeComponent();
            grafo = new CGrafo();
            nuevoNodo = null;
            var_control = 0;
            ventanaVertice = new Vertice();
            ventanaArco = new Arco();

            nodosRuta = new List<CVertice>();
            nodosOrdenados = new List<CVertice>();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
            ControlStyles.DoubleBuffer, true);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                foreach (CVertice nodo in grafo.nodos)
                {
                    if (nodo.Valor == comboBox1.SelectedItem.ToString())
                    {
                        grafo.nodos.Remove(nodo);
                        //Borrando arcos que posea el nodo eliminado
                        nodo.ListaAdyacencia = new List<CArco>();
                        break;
                    }
                }
                foreach (CVertice nodo in grafo.nodos)
                {
                    foreach (CArco arco in nodo.ListaAdyacencia)
                    {
                        if (arco.nDestino.Valor == comboBox1.SelectedItem.ToString())
                        {
                            nodo.ListaAdyacencia.Remove(arco);
                            break;
                        }
                    }
                }
                nuevoArco = true;
                nuevoVertice = true;
                comboBox1.SelectedIndex = -1;
                Pizarra.Refresh();
            }
            else
            {
                label5.Text = "Seleccione un nodo";
                label5.ForeColor = Color.Red;
            }
        }

        
        public void ordenarNodos()
        {
            nodosOrdenados = new List<CVertice>();
            bool est = false;
            foreach (CVertice nodo in grafo.nodos)
            {
                if (nodo.Valor == origen)
                {
                    nodosOrdenados.Add(nodo);
                    est = true;
                }
                else if (est)
                    nodosOrdenados.Add(nodo);
            }
            foreach (CVertice nodo in grafo.nodos)
            {
                if (nodo.Valor == origen)
                {
                    est = false;
                    break;
                }
                else if (est)
                    nodosOrdenados.Add(nodo);
            }
        }
        private void recorridoProfundidad(CVertice vertice, Graphics g)
        {
            vertice.Visitado = true;
            vertice.colorear(g);
            string p = "";
            Thread.Sleep(1000);
            vertice.DibujarVertice(g);
            foreach (CArco adya in vertice.ListaAdyacencia)
            {
                if (!adya.nDestino.Visitado) recorridoProfundidad(adya.nDestino, g);
                label7.Text += vertice.Valor.ToString() + ", "; // lloras por esas lineas 
            }
        }

        private void Pizarra_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) // Si se ha presionado el botón
            // izquierdo del mouse
            {
                if ((NodoOrigen = grafo.DetectarPunto(e.Location)) != null)
                {
                    var_control = 1; // recordemos que es usado para indicar el estado en la pizarra:
                    // 0 -> sin accion, 1 -> Dibujando arco, 2 -> Nuevo vértice
                }
                if (nuevoNodo != null && NodoOrigen == null)
                {
                    ventanaVertice.Visible = false;
                    ventanaVertice.control = false;
                    ventanaVertice.ShowDialog();
                    numeronodos = grafo.nodos.Count;//cuenta cuantos nodos hay en el grafo  
                    if (ventanaVertice.control)
                    {
                        if (grafo.BuscarVertice(ventanaVertice.dato) == null)
                        {
                            grafo.AgregarVertice(nuevoNodo);
                            nuevoNodo.Valor = ventanaVertice.dato;
                        }
                        else
                        {
                            label5.Text = "El Nodo " + ventanaVertice.dato + " ya existe en el grafo";
                            label5.ForeColor = Color.Red;
                        }
                    }
                    nuevoNodo = null;
                    nuevoVertice = true;
                    var_control = 0;
                    Pizarra.Refresh();
                }
                if (e.Button == System.Windows.Forms.MouseButtons.Right) // Si se ha presionado el botón
                // derecho del mouse
                {
                    if (var_control == 0)
                    {
                        if ((NodoOrigen = grafo.DetectarPunto(e.Location)) != null)
                        {
                            nuevoVerticeToolStripMenuItem.Text = "Nodo " + NodoOrigen.Valor;
                        }
                        else
                            Pizarra.ContextMenuStrip = this.contextMenuStrip1;
                    }
                }
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right) // Si se ha presionado el botón
            // derecho del mouse
            {
                if (var_control == 0)
                {
                    if ((NodoOrigen = grafo.DetectarPunto(e.Location)) != null)
                    {
                        nuevoVerticeToolStripMenuItem.Text = "Nodo " + NodoOrigen.Valor;
                    }
                    else
                        Pizarra.ContextMenuStrip = this.contextMenuStrip1;
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            int difX, difY;
            float distancia = 0;
            if (comboBox2.SelectedItem != null)
            {
                foreach (CVertice nodo in grafo.nodos)
                {
                    foreach (CArco arco in nodo.ListaAdyacencia)
                    {
                        if ("(" + nodo.Valor + "," + arco.nDestino.Valor + ") peso: " + arco.peso == comboBox2.SelectedItem.ToString())
                        {

                            int a = nodo.Posicion.X;
                            int a1 = nodo.Posicion.Y;
                            int b = arco.nDestino.Posicion.X;
                            int b1 = arco.nDestino.Posicion.Y;
                            difX = a - b;
                            difY = a1 - b1;
                            distancia = (float)Math.Sqrt((difX * difX + difY * difY));
                        }
                    }
                }
                MessageBox.Show("La distancia entre los nodos es de: " + distancia);
            }
            else
            {
                MessageBox.Show("Seleccione un arco entre los dos vertices");
            }
        }

        private int totalNodos; //lista de nodos
        int[] parent; // padre del nodo

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            eliminarnodo = new Eliminar(2);
            eliminarnodo.Visible = false;
            eliminarnodo.control = false;
            eliminarnodo.ShowDialog();
            if (eliminarnodo.control)
            {
                if (grafo.BuscarVertice(eliminarnodo.txteliminar.Text.Trim()) != null && grafo.BuscarVertice(eliminarnodo.txtelem.Text.Trim()) != null)
                {
                    //opc = 4;
                    //LblSimu.Text = "Simulacion: Algortimo de Warshall";
                    //double t = 0;
                    //Duracion.Restart();
                    calcularMatricesIniciales();
                    algoritmoWarshall();
                    obtenerRutaPesoWarshall(eliminarnodo.txteliminar.Text.Trim(), eliminarnodo.txtelem.Text.Trim());
                    if (buscarRuta)
                    {
                        for (int x = 0; x < nodosRuta.Count; x++)
                        {
                            grafo.Colorear(nodosRuta[x]);
                            if (x + 1 < nodosRuta.Count)
                            {
                                grafo.ColoArista(nodosRuta[x].Valor, nodosRuta[x + 1].Valor);
                                grafo.ColoArista(nodosRuta[x + 1].Valor, nodosRuta[x].Valor);
                            }
                        }
                        buscarRuta = false;
                    }
                    //Duracion.Stop();
                    //t = Duracion.ElapsedMilliseconds / 1000;
                    label2.Text = "El peso minimo entre los nodos es: " + peso.ToString();
                }
                else
                {
                    MessageBox.Show("El nodo No se encuentra en el grafo",
                    "Error Nodo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            rec.Visible = false;
            rec.control = false;
            rec.ShowDialog();
            if (grafo.BuscarVertice(rec.txtNodo.Text) != null)
            {
                grafo.Desmarcar();
                dijkstra(grafo.BuscarVertice(rec.txtNodo.Text));
            }
            else
            {
                MessageBox.Show("Ese Nodo no se encuentra en el grafo", "Error Nodo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void nuevoVerticeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nuevoNodo = new CVertice();
            var_control = 2; // recordemos que es usado para indicar el estado en la pizarra: 0 ->
            // sin accion, 1 -> Dibujando arco, 2 -> Nuevo vértice  
        }

        bool[] visitados;// variable para comprobar los nodos ya visitados

        private void calcularMatricesIniciales() // se calculan las matrices iniciales de distancia y de nodos
        {
            matrizDistanciaWarshall = new List<List<int>>();
            matrizFord = new List<List<int>>();
            matrizNodosWarshall = new List<List<int>>();
            nodosRuta = new List<CVertice>(); //lista de nodos
            totalNodos = grafo.nodos.Count; //cuenta el numero de nodos en la lista "nodos"
            parent = new int[totalNodos];
            visitados = new bool[totalNodos];
            //calculamos la matriz inicial de distancias
            for (int i = 0; i < totalNodos; i++)
            {
                List<int> filaDistancia = new List<int>();
                for (int j = 0; j < totalNodos; j++)
                {
                    //si el origen = al destino
                    if (i == j)
                    {
                        filaDistancia.Add(0);
                    }
                    else
                    {
                        //buscamos si existe la relacion i,j; de existir obtenemos la distancia
                        int distancia = -1;
                        for (int k = 0; k < grafo.nodos[i].ListaAdyacencia.Count; k++)
                        {
                            if (grafo.nodos[i].ListaAdyacencia[k].nDestino == grafo.nodos[j])
                                distancia = grafo.nodos[i].ListaAdyacencia[k].peso;
                        }
                        filaDistancia.Add(distancia);
                    }
                }
                matrizDistanciaWarshall.Add(filaDistancia);// obtenemos la matriz inicial de distancias
                matrizFord.Add(filaDistancia);
            }
            //calculamos la matriz inicial de nodos
            for (int i = 0; i < totalNodos; i++)
            {
                List<int> puntosIntermedios = new List<int>();
                for (int j = 0; j < totalNodos; j++)
                {
                    puntosIntermedios.Add(j);
                }
                matrizNodosWarshall.Add(puntosIntermedios);// obtenemos la matriz inicial de nodos
            }
        }

        private void recorridoAnchura(CVertice vertice, Graphics g, string destino)
        {

            vertice.Visitado = true;
            cola.Enqueue(vertice);
            vertice.colorear(g);
            Thread.Sleep(1000);
            vertice.DibujarVertice(g);
            if (vertice.Valor == destino)
            {
                nodoEncontrado = true;
                return;
            }
            while (cola.Count > 0)
            {
                CVertice aux = (CVertice)cola.Dequeue();
                foreach (CArco adya in aux.ListaAdyacencia)
                {
                    if (!adya.nDestino.Visitado)
                    {
                        if (!nodoEncontrado)
                        {
                            adya.nDestino.Visitado = true;
                            adya.nDestino.colorear(g);
                            Thread.Sleep(1000);
                            adya.nDestino.DibujarVertice(g);
                            if (destino != "")
                                distancia += adya.peso;
                            cola.Enqueue(adya.nDestino);
                            if (adya.nDestino.Valor == destino)
                            {
                                nodoEncontrado = true;
                                label9.Text += vertice.Valor.ToString() + ", "; //eso solo los pros como yo
                                return;
                            }
                        }
                    }
                }
            }
        }
        private void dijkstra(CVertice inicio)
        {
            if (inicio.ListaAdyacencia.Count != 0)
            {
                int n = grafo.nodos.Count;
                arreglo = new Label[numeronodos];
                arreglo2 = new Label[numeronodos];
                foreach (CVertice nodo in grafo.nodos)
                {
                    foreach (CArco a in nodo.ListaAdyacencia)
                    {
                        if (nodo == inicio)
                        {
                            a.nDestino.distancianodo = a.peso;
                            a.nDestino.pesoasignado = true;
                            a.color = Color.LimeGreen;
                            a.grosor_flecha = 4;
                        }
                        else if (nodo != inicio && a.nDestino.pesoasignado == false)
                        {
                            a.nDestino.distancianodo = Int32.MaxValue;
                        }
                    }
                }
                inicio.distancianodo = 0;
                inicio.Visitado = true;
                grafo.Colorear(inicio);
                Pizarra.Refresh();
                while (grafo.nododistanciaminima() != null)
                {
                    CVertice nododismin = grafo.nododistanciaminima();
                    nododismin.Visitado = true;
                    grafo.Colorear(nododismin);
                    Pizarra.Refresh();
                    foreach (CArco arco in nododismin.ListaAdyacencia)
                    {
                        if (arco.nDestino.distancianodo > nododismin.distancianodo + arco.peso)
                        {
                            if (arco.nDestino.pesoasignado)
                            { grafo.DibujarEntrantes(arco.nDestino); }
                            arco.nDestino.distancianodo = nododismin.distancianodo + arco.peso;
                            arco.nDestino.pesoasignado = true;
                            arco.color = Color.LimeGreen;
                            arco.grosor_flecha = 4;
                        }
                    };
                }
            }
            else
            {
                MessageBox.Show("El nodo que ha elegino no tiene nodos adyacentes"
                    , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void obtenerRutaPesoWarshall(string nodoOrigen, string nodoDestino)
        {
            int indexNodoOrigen = 0;
            int indexNodoDestino = 0;
            for (int i = 0; i < totalNodos; i++)//para i menor que la cantidad de nodos agregados a la cola
            {
                if (grafo.nodos[i].Valor == nodoOrigen)
                {
                    indexNodoOrigen = i;// el valor i sera el indice del nodo origen
                }
                if (grafo.nodos[i].Valor == nodoDestino)
                {
                    indexNodoDestino = i;// el valor j sera el indice del nodo destino
                }
            }
            List<int> ruta = new List<int>(); // se declara la lista ruta
            ruta.Add(indexNodoOrigen); // se añade el indice origen
            ruta.Add(indexNodoDestino);// se añade el indice destino
            obtenerNodosIntermedios(indexNodoOrigen, indexNodoDestino, ruta, 1); // se obtienen los nodos intermedios

            foreach (int nodo in ruta) // para cada nodo en ruta
            {
                nodosRuta.Add(grafo.nodos[nodo]);// agregara en nodosRuta cada nodo en la cola nodos del grafo
            }
            //obtenemos el peso de la ruta
            peso = matrizDistanciaWarshall[ruta[0]][ruta[ruta.Count - 1]];
            if (peso > -1)
            {
                buscarRuta = true;
            }
            else
            {
                MessageBox.Show("No se puede trazar ruta entre los nodos seleccionados");
            }
        }
        private void obtenerNodosIntermedios(int nodoOrigen, int nodoDestino, List<int> ruta, int indice) //metodo para obtener nodos intermedios
        {
            int intermedio = matrizNodosWarshall[nodoOrigen][nodoDestino];// la variable intermedio tendra el valor de la matriznodoWarshall con los valores de origen y destino
            if (intermedio != nodoDestino) //mientras intermedio sea diferente de nodo destino
            {
                ruta.Insert(indice, intermedio);// insertara en la lista ruta en el indice indicado
                indice++; //indice aumenta
                obtenerNodosIntermedios(intermedio, nodoDestino, ruta, indice); //obtiene nodo intermedio
            }
            else
            {
                indice--; // si intermedio y nodoDestino son iguales indice disminuye
                if (indice - 1 == -1)
                {
                    return;
                }
                nodoOrigen = ruta[indice - 1]; // nodo origen tendra el valor que tiene la ruta en determinado indice -1.
                nodoDestino = ruta[indice];  // nodo destino tendra el valor que tiene la ruta en determinado indice
                obtenerNodosIntermedios(nodoOrigen, nodoDestino, ruta, indice); // obtiene el nodo intermedio 
            }
        }
        private void algoritmoWarshall() //se declara el metodo de warshall
        {
            for (int k = 0; k < totalNodos; k++)
            {
                for (int i = 0; i < totalNodos; i++)
                {
                    for (int j = 0; j < totalNodos; j++)
                    {
                        //hacemos las operaciones siempre y cuando exista distancia con el intermediario k: [i,k,j]
                        //es decir,debe existir la distancia d(i,k) y d(k,j)
                        if (matrizDistanciaWarshall[i][k] > 0.0 && matrizDistanciaWarshall[k][j] > 0.0)
                        {
                            int distancia = matrizDistanciaWarshall[i][k] + matrizDistanciaWarshall[k][j];

                            if (matrizDistanciaWarshall[i][j] > 0.0)
                            {
                                if (matrizDistanciaWarshall[i][j] > distancia)
                                {
                                    matrizDistanciaWarshall[i][j] = distancia;
                                    matrizNodosWarshall[i][j] = k;
                                }
                            }
                            else
                            {
                                if (matrizDistanciaWarshall[i][j] < 0.0)
                                {
                                    matrizDistanciaWarshall[i][j] = distancia;
                                    matrizNodosWarshall[i][j] = k;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
