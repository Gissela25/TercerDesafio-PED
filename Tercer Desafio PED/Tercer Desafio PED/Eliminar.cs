using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tercer_Desafio_PED
{
    public partial class Eliminar : Form
    {
        public bool control;
        public string dato;
        public int estado;
        public Eliminar(int v)
        {
            InitializeComponent();
            control = false;
            dato = " ";
            estado = v;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            string valor = txteliminar.Text.Trim();
            string valor2 = txtelem.Text.Trim();
            if ((valor == "") || (valor == " "))
            {
                MessageBox.Show("Es necesario ingresar un valor", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);

            }
            if (estado == 2 && (valor2 == "" || valor2 == " "))
            {
                MessageBox.Show("Es necesario ingresar un valor", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
            else
            {
                control = true;
                this.Hide();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            control = false;
            this.Hide();
        }

        private void Eliminar_Load(object sender, EventArgs e)
        {
            txteliminar.Focus();
            this.Text = "Algoritmo Warshall";
            label1.Text = "Ruta de nodos:";
            label2.Text = "Nodo origen";
            label3.Text = "Nodo destino";
        }

        private void Eliminar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAceptar_Click(null, null);
            }
        }

        private void Eliminar_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void Eliminar_Shown(object sender, EventArgs e)
        {
            txteliminar.Clear();
            txteliminar.Focus();
        }
    }
}
