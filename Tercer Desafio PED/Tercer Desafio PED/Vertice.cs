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
    public partial class Vertice : Form
    {
        public bool control; // variable de control
        public string dato; // el dato que almacenará el vértice
        public Vertice()
        {
            InitializeComponent();
            control = false;
            dato = " ";
        }

        private void Vertice_Load(object sender, EventArgs e)
        {
            txtVertice.Focus();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            dato = txtVertice.Text.Trim();
            if ((dato == "") || (dato == " "))//si no se ha ingresado un dato previamente
            {
                MessageBox.Show("Debes ingresar un valor", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
            else if (dato.Length > 2)
            {
                var result = MessageBox.Show("Se tomaran en cuenta solamente los dos primeros carateres. Desea continuar?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    dato = dato.Substring(0, 2);
                    control = true;
                    Hide();
                }
            }
            else
            {
                control = true;
                Hide();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            control = false;
            Hide();
        }

        private void Vertice_Shown(object sender, EventArgs e)
        {

            txtVertice.Clear();
            txtVertice.Focus();
        }

        private void Vertice_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void txtVertice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)//llave primaria del textboxt
            {
                btnAceptar_Click(null, null);
            }
        }
    }
}
