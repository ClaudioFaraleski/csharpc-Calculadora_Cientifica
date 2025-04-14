global using System;
global using System.Drawing;
global using System.Windows.Forms;

namespace CalculadoraApp
{
    public class FormCalculadora : Form
    {
        private TextBox txtDisplay;
        private string operacaoAtual = "";
        private double numero1 = 0;
        private bool novoNumero = true;

        public FormCalculadora()
        {
            this.Text = "Calculadora Científica - Autor: Claudio Faraleski";
            this.Width = 500;
            this.Height = 500;
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.StartPosition = FormStartPosition.CenterScreen;

            txtDisplay = new TextBox
            {
                Width = 460,
                Height = 40,
                Top = 20,
                Left = 10,
                Font = new System.Drawing.Font("Arial", 20),
                TextAlign = HorizontalAlignment.Right,
                ReadOnly = true,
                Text = "0",
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            string[,] botoes = {
                {"√", "x²", "%", "CE", "/"},
                {"7", "8", "9", "±", "*"},
                {"4", "5", "6", "1/x", "-"},
                {"1", "2", "3", "π", "+"},
                {"0", ",", "=", "e", "C"}
            };

            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    Button btn = new Button
                    {
                        Text = botoes[row, col],
                        Width = 60,
                        Height = 60,
                        Top = row * 70 + 80,
                        Left = col * 70 + 10,
                        FlatStyle = FlatStyle.Flat,
                        BackColor = "+-*/=".Contains(botoes[row, col]) ? Color.FromArgb(255, 128, 0) 
                                : "√x²%±π".Contains(botoes[row, col]) ? Color.FromArgb(0, 128, 255)
                                : Color.White,
                        ForeColor = "+-*/=√x²%±π".Contains(botoes[row, col]) ? Color.White : Color.Black,
                        Font = new Font("Arial", 14, FontStyle.Bold)
                    };

                    if ("0123456789,".Contains(btn.Text))
                        btn.Click += Numero_Click;
                    else if ("+-*/".Contains(btn.Text))
                        btn.Click += Operacao_Click;
                    else if (btn.Text == "=")
                        btn.Click += Igual_Click;
                    else if ("√x²%±π1/xe".Contains(btn.Text))
                        btn.Click += Funcao_Click;
                    else if (btn.Text == "CE" || btn.Text == "C")
                        btn.Click += Limpar_Click;

                    this.Controls.Add(btn);
                }
            }

            this.Controls.Add(txtDisplay);
        }

        private void Numero_Click(object? sender, EventArgs e)
        {
#pragma warning disable CS8600 // Conversão de literal nula ou possível valor nulo em tipo não anulável.
            Button btn = (Button)sender;
#pragma warning restore CS8600 // Conversão de literal nula ou possível valor nulo em tipo não anulável.
            if (novoNumero)
            {
#pragma warning disable CS8602 // Desreferência de uma referência possivelmente nula.
                txtDisplay.Text = btn.Text;
#pragma warning restore CS8602 // Desreferência de uma referência possivelmente nula.
                novoNumero = false;
            }
            else if (txtDisplay.Text.Length < 12)
            {
#pragma warning disable CS8602 // Desreferência de uma referência possivelmente nula.
                if (btn.Text == "," && txtDisplay.Text.Contains(","))
                    return;
#pragma warning restore CS8602 // Desreferência de uma referência possivelmente nula.
                txtDisplay.Text += btn.Text;
            }
        }

        private void Operacao_Click(object? sender, EventArgs e)
        {
            try
            {
#pragma warning disable CS8600 // Conversão de literal nula ou possível valor nulo em tipo não anulável.
                Button btn = (Button)sender;
#pragma warning restore CS8600 // Conversão de literal nula ou possível valor nulo em tipo não anulável.
                numero1 = Convert.ToDouble(txtDisplay.Text);
#pragma warning disable CS8602 // Desreferência de uma referência possivelmente nula.
                operacaoAtual = btn.Text;
#pragma warning restore CS8602 // Desreferência de uma referência possivelmente nula.
                novoNumero = true;
            }
            catch (Exception)
            {
                txtDisplay.Text = "Erro";
                novoNumero = true;
            }
        }

        private void Igual_Click(object? sender, EventArgs e)
        {
            try
            {
                double numero2 = Convert.ToDouble(txtDisplay.Text);
                double resultado = 0;

                switch (operacaoAtual)
                {
                    case "+": resultado = numero1 + numero2; break;
                    case "-": resultado = numero1 - numero2; break;
                    case "*": resultado = numero1 * numero2; break;
                    case "/":
                        if (numero2 == 0)
                            throw new DivideByZeroException();
                        resultado = numero1 / numero2;
                        break;
                }

                if (double.IsInfinity(resultado) || double.IsNaN(resultado))
                    throw new OverflowException();

                txtDisplay.Text = resultado.ToString("N8").TrimEnd('0').TrimEnd(',');
                novoNumero = true;
            }
            catch (DivideByZeroException)
            {
                txtDisplay.Text = "Erro: Divisão por zero";
                novoNumero = true;
            }
            catch (Exception)
            {
                txtDisplay.Text = "Erro";
                novoNumero = true;
            }
        }

        private void Funcao_Click(object? sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender!;
                double numero = Convert.ToDouble(txtDisplay.Text);
                double resultado = 0;

                switch (btn.Text)
                {
                    case "√":
                        if (numero < 0) throw new ArgumentException("Raiz de número negativo");
                        resultado = Math.Sqrt(numero);
                        break;
                    case "x²":
                        resultado = Math.Pow(numero, 2);
                        break;
                    case "%":
                        resultado = numero / 100;
                        break;
                    case "±":
                        resultado = -numero;
                        break;
                    case "1/x":
                        if (numero == 0) throw new DivideByZeroException();
                        resultado = 1 / numero;
                        break;
                    case "π":
                        resultado = Math.PI;
                        break;
                    case "e":
                        resultado = Math.E;
                        break;
                }

                if (double.IsInfinity(resultado) || double.IsNaN(resultado))
                    throw new OverflowException();

                txtDisplay.Text = resultado.ToString("N8").TrimEnd('0').TrimEnd(',');
                novoNumero = true;
            }
            catch (DivideByZeroException)
            {
                txtDisplay.Text = "Erro: Divisão por zero";
                novoNumero = true;
            }
            catch (ArgumentException ex)
            {
                txtDisplay.Text = "Erro: " + ex.Message;
                novoNumero = true;
            }
            catch (Exception)
            {
                txtDisplay.Text = "Erro";
                novoNumero = true;
            }
        }

        private void Limpar_Click(object? sender, EventArgs e)
        {
            txtDisplay.Text = "0";
            novoNumero = true;
            numero1 = 0;
            operacaoAtual = "";
        }
    }
}
