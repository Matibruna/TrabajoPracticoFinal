﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// Author: Bruna Matias Mail & GitHub: matibruna6@gmail.com
/// Trabajo Final Simulacion Universidad Tecnologica Nacional, Facultad Regional Cordoba.
/// Legajo 77682.
/// 

namespace TrabajoPracticoFinal
{
    public partial class Main : Form
    {
        Random rnd;

        //Demanda Dias Soleados
        private double ProbDemDiaSoleado1, ProbDemDiaSoleado2, ProbDemDiaSoleado3, ProbDemDiaSoleado4;

        //Demanda Dias Nublados
        private double ProbDemDiaNublado1, ProbDemDiaNublado2, ProbDemDiaNublado3, ProbDemDiaNublado4, ProbDemDiaNublado5;

        //Probabilidad Dia Soleado/Nublado
        private double ProbDiaSoleado, ProbDiaNublado;

        //Precios y Cantidades
        private bool satisfacerDemanda;
        private bool comprarCantidadDemandada;
        private double precioSatisfacerDemanda;
        private double docPorPed;

        private double cantidadCajonesComprar;
        private double precioCajon;

        private double precioVentaUnitario;

        //Cantidad de dias a simular y mostrar.
        private double simularDesd;
        private double simularHast;
        private double diasSimular;

        //Acumuladores
        private double acumuladorGananciaTotal;
        private double acumuladorRosasVendidasCementerio;
        private double acumuladorDemanda;

        public Main()
        {
            InitializeComponent();
            recalcularDiasNublados();
            recalcularDiasSoleados();
            recalcularProbDiasSoleados();
        }

        private void recalcularDiasNublados()
        {
            try
            {
                ProbDemDiaNublado1 = Double.Parse(pdn1.Value.ToString());
                ProbDemDiaNublado2 = Double.Parse(pdn2.Value.ToString());
                ProbDemDiaNublado3 = Double.Parse(pdn3.Value.ToString());
                ProbDemDiaNublado4 = Double.Parse(pdn4.Value.ToString());
                ProbDemDiaNublado5 = Double.Parse(pdn5.Value.ToString());


                double total = ProbDemDiaNublado1 + ProbDemDiaNublado2 + ProbDemDiaNublado3 + ProbDemDiaNublado4 + ProbDemDiaNublado5; 

                totalDN.Text = total.ToString();
            }
            catch
            {

            }
        }

        private void recalcularDiasSoleados()
        {
            try
            {
                ProbDemDiaSoleado1 = Double.Parse(ds1.Value.ToString());
                ProbDemDiaSoleado2 = Double.Parse(ds2.Value.ToString());
                ProbDemDiaSoleado3 = Double.Parse(ds3.Value.ToString());
                ProbDemDiaSoleado4 = Double.Parse(ds4.Value.ToString());

                double total = ProbDemDiaSoleado1 + ProbDemDiaSoleado2 + ProbDemDiaSoleado3 + ProbDemDiaSoleado4;

                totalDS.Text = total.ToString();
            }
            catch
            {

            }
        }

        private void recalcularProbDiasSoleados()
        {
            try
            {
                ProbDiaNublado = Double.Parse(pDiaNublado.Value.ToString());
                pDiaSoleado.Value = Convert.ToDecimal(1 - ProbDiaNublado);
                ProbDiaSoleado = 1 - ProbDiaNublado;

                double total = ProbDiaSoleado + ProbDiaNublado;

                tpDias.Text = total.ToString();
            }
            catch
            {

            }
        }

        private void recalcularProbDiasNublados() 
        {
            try
            {
                ProbDiaSoleado = Double.Parse(pDiaSoleado.Value.ToString());
                pDiaNublado.Value = Convert.ToDecimal(1 - ProbDiaSoleado);
                ProbDiaNublado = 1 - ProbDiaSoleado;
                

                double total = ProbDiaSoleado + ProbDiaNublado;

                tpDias.Text = total.ToString();
            }
            catch
            {

            }
        }

        private void diasASimular_TextChanged(object sender, EventArgs e)
        {
            simularHasta.Value= diasASimular.Value;
        }

        private void ds4_TextChanged(object sender, EventArgs e)
        {
            recalcularDiasSoleados();
        }

        private void ds3_TextChanged(object sender, EventArgs e)
        {
            recalcularDiasSoleados();
        }

        private void ds2_TextChanged(object sender, EventArgs e)
        {
            recalcularDiasSoleados();
        }

        private void ds1_TextChanged(object sender, EventArgs e)
        {
            recalcularDiasSoleados();
        }

        private void dn5_TextChanged(object sender, EventArgs e)
        {
            recalcularDiasNublados();
        }

        private void dn4_TextChanged(object sender, EventArgs e)
        {
            recalcularDiasNublados();
        }

        private void dn3_TextChanged(object sender, EventArgs e)
        {
            recalcularDiasNublados();
        }

        private void dn2_TextChanged(object sender, EventArgs e)
        {
            recalcularDiasNublados();
        }

        private void dn1_TextChanged(object sender, EventArgs e)
        {
            recalcularDiasNublados();
        }

        private void pDiaSoleado_TextChanged(object sender, EventArgs e)
        {
            recalcularProbDiasNublados();
        }

        private void pDiaNublado_TextChanged(object sender, EventArgs e)
        {
            recalcularProbDiasSoleados();
        }

        private void rbDemandaDiaAnterior_CheckedChanged(object sender, EventArgs e)
        {
            cajonesAPedir.Enabled = !rbDemandaDiaAnterior.Checked;
            demandaInicial.Enabled = rbDemandaDiaAnterior.Checked;
        }

        private void btnSimulacion_Click(object sender, EventArgs e)
        {

            if (totalDN.Text != "1" || totalDS.Text != "1" || tpDias.Text != "1") {
                DialogResult r = MessageBox.Show("La suma de las probabilidades no es 1. ¿Desea Continuar?", "Iniciar simulacion.", MessageBoxButtons.YesNo);
                if (r == DialogResult.Yes) { }
                else if (r == DialogResult.No){ return; }
            }

            dgv.Rows.Clear();
            satisfacerDemanda = rbSatisfacerDemanda.Checked;
            comprarCantidadDemandada = rbDemandaDiaAnterior.Checked;

            // Setear los acumuladores a 0.
            acumuladorGananciaTotal = 0;
            acumuladorRosasVendidasCementerio = 0;
            acumuladorDemanda = 0;

            // Obtener datos de simulacion.
            precioCajon = Double.Parse(precioPedido.Text) * Double.Parse(docenasPorPedido.Text);
            precioSatisfacerDemanda = Double.Parse(sdPrecio.Text);
            precioVentaUnitario = Double.Parse(ventaPrecioUnitario.Text);
            simularDesd = Double.Parse(simularDesde.Text);
            simularHast = Double.Parse(simularHasta.Text);
            diasSimular = Double.Parse(diasASimular.Text);
            docPorPed = Double.Parse(docenasPorPedido.Text);


            // Simular, 2 formas:

            simulacionPorFilas(comprarCantidadDemandada);

            //simulacionTradicional();
        }

        private void rbSatisfacerDemanda_CheckedChanged(object sender, EventArgs e)
        {
            sdPrecio.Enabled = rbSatisfacerDemanda.Checked;
        }

        //
        // Simulacion un poco mas tradicional en un solo loop con variables y no filas construidas.
        private void simulacionTradicional()
        {
            if (comprarCantidadDemandada)
            {
                cantidadCajonesComprar = Math.Ceiling(Int32.Parse(demandaInicial.Text) / docPorPed);
            }
            else
            {
                cantidadCajonesComprar = Double.Parse(cajonesAPedir.Text);
            }

            dgv.Rows.Add(-1.0, 0.0, 0.0, 0.0, "Inicio de simulacion", 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
            for (double i = 0; i < diasSimular; i++)
            {
                simularFilaTradicional(satisfacerDemanda, comprarCantidadDemandada, i);
            }

            dgv.Rows.Add(diasSimular, 0.0, 0.0, 0.0, "Fin de simulacion", 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, Math.Round(acumuladorGananciaTotal, 2), Math.Round(acumuladorRosasVendidasCementerio, 2), Math.Round(acumuladorDemanda, 2));

            // Mostramos los resultados finales en la interfaz.
            gananciasPromedio.Text = (Math.Round(acumuladorGananciaTotal / diasSimular, 2)).ToString();
            txtRosasVendidasCem.Text = (Math.Round(acumuladorRosasVendidasCementerio / diasSimular, 2)).ToString();
            txtDemandaAcumulada.Text = (Math.Round(acumuladorDemanda / diasSimular), 2).ToString();
        }
        private void simularFilaTradicional(bool satisfacerDemanda, bool comprarCantidadDemandada, double i)
        {
            
            rnd = new Random();
            string clima;
            double randomClima, randomDemanda;
            double demanda;
            double ventasCementerio = 0;
            double cantidadCompradaParaSatisfacerDemanda=0;

            //Costos
            double costoCompra, costoFaltante=0, costoExtra=0;

            //Ganancias
            double gananciaPorVentas, gananciaTotal, gananciaVentasCementerio=0;

            // Tirada random clima.
            randomClima = rnd.NextDouble();

            // Buscamos el clima asociado al random anterior.
            clima = getClima(randomClima);

            // Tirada random demanda.
            randomDemanda = rnd.NextDouble();

            // Buscamos la demanda, primero preguntando si el dia fue soleado o nublado.
            if (clima == "Soleado")
            {
                demanda = getDemandaSoleado(randomDemanda);
            }
            else
            {
                demanda = getDemandaNublado(randomDemanda);
            }

            if ((cantidadCajonesComprar * docPorPed * 12) >= demanda)
            {
                ventasCementerio = cantidadCajonesComprar * docPorPed * 12 - demanda;

                gananciaPorVentas = demanda * precioVentaUnitario;

                gananciaVentasCementerio = Double.Parse(txtPrecioVentaCementerio.Text) * ventasCementerio;
            }
            else
            {
                // Opcion Satisfacer Demanda a Mayor precio ¿Activa?
                if (satisfacerDemanda)
                {
                    // En caso de estar activa, calcular la cantidad de docenas a comprar.
                    double docenasAComprar = Math.Ceiling((demanda - (cantidadCajonesComprar * docPorPed * 12))/12);
                    cantidadCompradaParaSatisfacerDemanda = docenasAComprar * 12;

                    // Modificar la cantidad disponible.
                    double cantidadDisponible = (cantidadCajonesComprar * docPorPed * 12) + cantidadCompradaParaSatisfacerDemanda;

                    // Calcular el costo que supone la compra extra de rosas.
                    costoExtra = docenasAComprar * precioSatisfacerDemanda;

                    // Si compramos mas flores, ¿Sobran?
                    if (cantidadDisponible > demanda)
                    {
                        // En caso de que sobren:
                        // Se calculan cuantas se venderan al cementerio.
                        ventasCementerio = cantidadDisponible - demanda;
                        // Y se calculan las ganancias asociadas a las ventas generales y a las ventas al cementerio.
                        // Ventas Generales
                        gananciaPorVentas = demanda * precioVentaUnitario;
                        //Ventas al Cementerio.
                        gananciaVentasCementerio = Double.Parse(txtPrecioVentaCementerio.Text) * ventasCementerio;
                    } 
                    else 
                    {
                        // En caso de que no sobren flores.
                        // Ventas y Ganancias respecto al cementerio siguen siendo 0.

                        // Solo se calcula las ganancias por ventas generales.
                        gananciaPorVentas = demanda * precioVentaUnitario;
                    }
                }
                else
                {
                    // No se satisface la demanda.
                    // Ventas al cementerio son 0, ya que la demanda es > que la cantidad disponible, por lo tanto 0 vendidas y ganancia 0.(cementerio)

                    // Se calculan las ganancias por ventas generales.
                    gananciaPorVentas = (cantidadCajonesComprar * docPorPed * 12) * precioVentaUnitario; 

                    // Y un costo por faltante de rosas.
                    costoFaltante = (demanda - cantidadCajonesComprar * docPorPed * 12) * Double.Parse(costoFaltanteUnitario.Text);
                }
            }

            // Calculamos lo que costo comprar las rosas iniciales (No el caso de satisfacer demanda, ese caso ya fue calculado anteriormente).
            costoCompra = cantidadCajonesComprar * precioCajon;

            // Se calculan las ganancias totales como sumatoria de ganancias - costos.
            gananciaTotal = gananciaPorVentas + gananciaVentasCementerio - costoCompra - costoFaltante - costoExtra;

            // Se actualizan los acumuladores.
            acumuladorRosasVendidasCementerio += ventasCementerio;
            acumuladorGananciaTotal += gananciaTotal;
            acumuladorDemanda += demanda;

            // Se agrega a la tabla en caso de que se deba agregar.
            if (simularDesd <= i && i < simularHast) 
            {
                dgv.Rows.Add(i, cantidadCajonesComprar*docPorPed*12, cantidadCompradaParaSatisfacerDemanda, Math.Round(randomClima, 2), clima, Math.Round(randomDemanda, 2), demanda, gananciaPorVentas, ventasCementerio, Math.Round(gananciaVentasCementerio, 2), costoCompra, costoExtra, Math.Round(costoFaltante, 2), Math.Round(gananciaTotal, 2), Math.Round(acumuladorGananciaTotal, 2), Math.Round(acumuladorDemanda, 2));
            }

            if (comprarCantidadDemandada)
            {
                //¿Compra constante o compra la cantidad demandada el dia anterior?//
                // Compra constante = no pasa nada.
                // Compra la cantidad demandada el dia anterior:
                // Se redondea la demanda del dia anterior a la minima cantidad de cajones posibles por comprar para satisfacer esa demanda.
                // Ejemplo:
                // Demanda 48 Rosas, Con 1 Cajon de 4 Docenas Alcanza.
                // Demanda 50 Rosas, Con 1 no alcanza, la demanda sera de 2 cajones.

                double NuevaDemanda = (demanda/12) / docPorPed;
                cantidadCajonesComprar = Math.Ceiling( NuevaDemanda );
            }
           
        }

        //
        // Simulacion con 2 filas en memoria.
        private void simulacionPorFilas(bool comprarCantidadDemandada)
        {
            // Variables Para Filas N y N+1
            Fila filaAnterior = new Fila(0);
            // Seteamos los acumuladores en 0 para no pasar null por parametro en la primer iteracion.
            filaAnterior.setGananciaTotalAcumulada(0);
            filaAnterior.setDemandaAcumulada(0);
            filaAnterior.setRosasVendidasCementerioAcumulada(0);

            Fila filaSiguiente;

            // Primer Chequeo, en caso de que la opcion comprar cantidad demandada el dia anterior este habilitada, se toma como que el dia anterior hubo una demanda de 8 docenas de flores.
            if (comprarCantidadDemandada)
            {
                cantidadCajonesComprar = Math.Ceiling( Convert.ToDouble(demandaInicial.Value) / docPorPed );
            }
            else
            {
                cantidadCajonesComprar = Convert.ToDouble(cajonesAPedir.Value);
            }

            // Se genera la primer fila de inicio.
            dgv.Rows.Add(-1.0, 0.0, 0.0, 0.0, "Inicio de simulacion", 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);


            // Loop de simulacion.
            for (double i = 0; i < diasSimular; i++)
            {
                // SatisfacerDemanda es una variable global booleana que determina si se realizara o no la compra mas cara de flores para satisfacer la demanda.
                filaSiguiente = generarFila(satisfacerDemanda, i, cantidadCajonesComprar * docPorPed * 12, filaAnterior.getGananciaTotalAcumulada(), filaAnterior.getRosasVendidasCementerioAcumulada(), filaAnterior.getDemandaAcumulada());

                if (comprarCantidadDemandada)
                {
                    // Si la opcion comprar cantidad demandada el dia anterior esta activa:
                    
                    // Se calcula la cantidad de cajones que haran falta.
                    // Cada cajon tiene "docPorPed" docenas.
                    // filaAnterior.demanda / 12 nos da las docenas. y dividiendo por las docenasPorPedido nos da la cantidad de cajones a comprar, puede dar numeros con coma.
                    double NuevaDemanda = (filaSiguiente.getDemanda() / 12) / docPorPed;

                    // Como no se puede comprar 0.5 cajones o 0.3 cajones, se redondea hacia arriba.
                    cantidadCajonesComprar = Math.Ceiling(NuevaDemanda);
                }

                // ¿Se ingresan los datos a la tabla?
                if (simularDesd <= i && i < simularHast)
                {
                    /// Agregar a la tabla.
                    dgv.Rows.Add(i, filaSiguiente.getCantidadComprada(), filaSiguiente.getCantidadCompradaSatisfacerDemanda(), Math.Round(filaSiguiente.getRndClima(), 2), filaSiguiente.getClima(), Math.Round(filaSiguiente.getRndDemanda(), 2), filaSiguiente.getDemanda(), filaSiguiente.getGananciaVentas(), filaSiguiente.getVentasCementerio(), Math.Round(filaSiguiente.getGananciaVentasCementerio(), 2), filaSiguiente.getCostoCompra(), filaSiguiente.getCostoCompraSatisfacerDemanda(), Math.Round(filaSiguiente.getCostoFaltante(), 2), Math.Round(filaSiguiente.getGananciaTotal(), 2), Math.Round(filaSiguiente.getGananciaTotalAcumulada(), 2), filaSiguiente.getRosasVendidasCementerioAcumulada(), filaSiguiente.getDemandaAcumulada());
                }

                filaAnterior = filaSiguiente;
            }
            
            // Se agrega a la tabla una fila final.
            dgv.Rows.Add(diasSimular, 0.0, 0.0, 0.0, "Fin de simulacion", 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, Math.Round(filaAnterior.getGananciaTotalAcumulada(), 2), filaAnterior.getRosasVendidasCementerioAcumulada(), filaAnterior.getDemandaAcumulada());

            // Se muestran los resultados en los textBox de la interfaz.
            gananciasPromedio.Text = (Math.Round(filaAnterior.getGananciaTotalAcumulada() / diasSimular, 2)).ToString();
            txtRosasVendidasCem.Text = (Math.Round(filaAnterior.getRosasVendidasCementerioAcumulada() / diasSimular, 2)).ToString();
            txtDemandaAcumulada.Text = (Math.Round(filaAnterior.getDemandaAcumulada() / diasSimular, 2)).ToString();

        }

        private Fila generarFila(bool satisfacerDemanda, double i, double cantidadComprada, double acumuladoGanancias, double acumuladoRosas, double acumuladorDemanda)
        {
            rnd = new Random();

            Fila filaSimulacion = new Fila(i);

            // Set Random Clima
            filaSimulacion.setRndClima(rnd.NextDouble());
            // Set Clima, Utilizando funcion getClima(randomCLima) y el randomClima = filaSimulacion.getRndClima();
            filaSimulacion.setClima(getClima(filaSimulacion.getRndClima()));

            // Set Cantidad a Comprar, parametro de la funcion generarFila.
            filaSimulacion.setCantidadComprada(cantidadComprada);

            // Set Random Demanda
            filaSimulacion.setRndDemanda(rnd.NextDouble());

            // Set Demanda, Utiliza funcion getDemanda, primero verificando si el clima es soleado o nublado.
            if (filaSimulacion.getClima() == "Soleado")
            {
                // Misma estructura que en el clima.
                // Funcion setDemanda(double Demanda)
                // Donde la demanda se busca en una funcion getDemandaSoleado(double randomDemanda)
                // y el randomDemanda se busca de filaSimulacion.getRndDemanda() que retorna el valor.
                filaSimulacion.setDemanda(getDemandaSoleado(filaSimulacion.getRndDemanda()));
            }
            else
            {
                // Misma estructura que en el clima.
                // Funcion setDemanda(double Demanda)
                // Donde la demanda se busca en una funcion getDemandaSoleado(double randomDemanda)
                // y el randomDemanda se busca de filaSimulacion.getRndDemanda() que retorna el valor.
                filaSimulacion.setDemanda(getDemandaNublado(filaSimulacion.getRndDemanda()));
            }

            if (filaSimulacion.getCantidadComprada() >= filaSimulacion.getDemanda())
            {
                // El stock es mayor a la demanda.

                // Se calcula cuanto es el extra de stock que no se vendera a precio normal ya que la demanda es menor al stock.
                double ventasCementerio = filaSimulacion.getCantidadComprada() - filaSimulacion.getDemanda();

                // Se setean las ventas al cementerio en la filaSimulacion.
                filaSimulacion.setVentasCementerio(ventasCementerio, Double.Parse(txtPrecioVentaCementerio.Text));

                // Como la demanda es mayor a lo disponible se vendera todo lo que se demande.
                // precioVentaUnitario es una variable global.
                double gananciaPorVentas = filaSimulacion.getDemanda() * precioVentaUnitario;

                // Se setean las ganancias por ventas en la filaSimulacion.
                filaSimulacion.setGananciasVentas(gananciaPorVentas);
            }
            else
            {
                // La demanda es mayor al stock.

                // Condicion externa:
                // ¿Satisfacer demanda a un precio mayor?
                if (satisfacerDemanda)
                {
                    // Se debe satisfacer la demanda:

                    // Calculamos la cantidad de docenas a comprar:
                    double docenasAComprar = Math.Ceiling((filaSimulacion.getDemanda() - filaSimulacion.getCantidadComprada()) / 12);

                    // Seteamosen filaSimulacion la cantidad de rosas que se compraran para satisfacer la demanda.
                    filaSimulacion.setCantidadCompradaSatisfacerDemanda(docenasAComprar * 12);

                    // Se calcula la nueva cantidad disponible.
                    double cantidadDisponible = filaSimulacion.getCantidadComprada() + filaSimulacion.getCantidadCompradaSatisfacerDemanda();

                    // Se calcula el costo extra de comprar a un precio mayor por docena.
                    double costoExtra = docenasAComprar * precioSatisfacerDemanda;

                    // Seteamos el precio en la filaSimulacion.
                    filaSimulacion.setCostoCompraSatisfacerDemanda(costoExtra);


                    // Preguntamos, con la nueva cantidad disponible, ¿nos sobra stock o se vende todo?
                    // Como se repuso stock para satisfacer la demanda no es posible que la demanda sea menor a la cantidadDisponible.
                    if (cantidadDisponible > filaSimulacion.getDemanda())
                    {
                        // Seteamos las ventas que se realizaran al cementerio
                        // En caso de que el nuevo disponible sea mayor a la demanda.
                        filaSimulacion.setVentasCementerio(cantidadDisponible - filaSimulacion.getDemanda(), Double.Parse(txtPrecioVentaCementerio.Text));
                    }
                    else
                    {
                        // El caso de que la cantidad disponible no es mayor a la demanda indica que la demanda es igual a la cantidad disponible. 
                        // Ventas al cementerio = 0, ya que no sobraron flores.
                        filaSimulacion.setVentasCementerio(0, Double.Parse(txtPrecioVentaCementerio.Text));
                    }

                    // Seteamos las ganancias por ventas, como nos aseguramos de que la demanda fue satisfecha, la cantidad vendida sera demanda * precioVentaUnitario.
                    // precioVentaUnitario es una variable global.
                    filaSimulacion.setGananciasVentas(filaSimulacion.getDemanda() * precioVentaUnitario);

                    // Seteamos el costo de faltante, como la demanda fue satisfecha el costo es 0.
                    filaSimulacion.setCostoFaltante(0);
                }
                else
                {
                    // Else, no se debe satisfacer la demanda.
                    filaSimulacion.setCantidadCompradaSatisfacerDemanda(0);
                    filaSimulacion.setCostoCompraSatisfacerDemanda(0);

                    // Como la demanda es mayor a la cantidad comprada, la cantidad vendida es la cantidad comprada.
                    // PrecioVentaUnitario es una variable global.
                    filaSimulacion.setGananciasVentas(filaSimulacion.getCantidadComprada() * precioVentaUnitario);

                    // Como Demanda > CantidadComprada se calcula un costo de faltante de stock:
                    filaSimulacion.setCostoFaltante((filaSimulacion.getDemanda() - filaSimulacion.getCantidadComprada()) * Double.Parse(costoFaltanteUnitario.Text));
                }
            }

            // cantidadCajonesComprar y precioCajon son variables globales.
            filaSimulacion.setCostoCompra(cantidadCajonesComprar * precioCajon);

            // Calculamos la ganancia total.
            double gananciaTotal = filaSimulacion.getGananciaVentas() + filaSimulacion.getGananciaVentasCementerio() - filaSimulacion.getCostoCompra() - filaSimulacion.getCostoFaltante() - filaSimulacion.getCostoCompraSatisfacerDemanda();

            // Se setean la ganancia total.
            filaSimulacion.setGananciaTotal(gananciaTotal);

            // Se setean los acumulados con las ganancias, demanda y rosas vendidas en esta iteracion como la suma de la iteracion actual + los acumulados anteriores que fueron pasados por parametro.
            filaSimulacion.setGananciaTotalAcumulada(acumuladoGanancias + gananciaTotal);
            filaSimulacion.setRosasVendidasCementerioAcumulada(filaSimulacion.getVentasCementerio() + acumuladoRosas);
            filaSimulacion.setDemandaAcumulada(acumuladorDemanda + filaSimulacion.getDemanda());

            return filaSimulacion;
        }

        class Fila
        {
            double nroFila;
            double cantidadComprada;
            double cantidadCompradaSatisfacerDemanda;
            double rndClima;
            string clima;
            double rndDemanda;
            double demanda;
            double gananciasVentas;
            double ventasCementerio;
            double gananciasVentCem;
            double costoCompra;
            double costoCompraSatisfacerDemanda;
            double costoFaltante;
            double gananciaTotal;
            double gananciaTotalAcumulada;
            double rosasVendidasCementerioAcumuladas;
            double demandaAcumulada;

            public void setDemandaAcumulada(double demandaAcum)
            {
                this.demandaAcumulada = demandaAcum;
            }

            public double getDemandaAcumulada()
            {
                return demandaAcumulada;
            }

            public Fila(double nroFila) 
            { 
                this.nroFila = nroFila; 
            }

            public void setCantidadComprada(double cantidadAComprar) 
            {
                this.cantidadComprada = cantidadAComprar;
            }

            public void setRndClima(double rnd)
            {
                this.rndClima = rnd;
            }

            public void setCantidadCompradaSatisfacerDemanda(double cant)
            {
                this.cantidadCompradaSatisfacerDemanda = cant;
            }

            public double getCantidadCompradaSatisfacerDemanda()
            {
                return cantidadCompradaSatisfacerDemanda;
            }

            public void setClima(string clima)
            {
                this.clima = clima;
            }

            public void setRndDemanda(double rnd)
            {
                this.rndDemanda = rnd;
            }

            public void setDemanda(double dem)
            {
                this.demanda = dem;
            }

            public void setGananciasVentas(double gan)
            {
                this.gananciasVentas = gan;
            }

            public void setVentasCementerio(double ventasCem, double precioVentaUnitario)
            {
                this.ventasCementerio = ventasCem;
                this.gananciasVentCem = ventasCem * precioVentaUnitario;
            }

            public void setGananciasVentasCementerio(double ventasCem)
            {
                this.gananciasVentCem = ventasCem;
            }

            public void setCostoCompra(double costoCompra)
            {
                this.costoCompra = costoCompra;
            }

            public void setCostoCompraSatisfacerDemanda(double costo)
            {
                this.costoCompraSatisfacerDemanda = costo;
            }

            public void setCostoFaltante(double costoFaltante)
            {
                this.costoFaltante = costoFaltante;
            }

            public void setGananciaTotal(double gananciaT) 
            {
                this.gananciaTotal = gananciaT;
            }

            public void setGananciaTotalAcumulada(double ganTotAcum)
            {
                this.gananciaTotalAcumulada = ganTotAcum;
            }

            public double getGananciaTotalAcumulada()
            {
                return gananciaTotalAcumulada;
            }

            public double getRndClima()
            {
                return rndClima;
            }

            public double getRndDemanda()
            {
                return rndDemanda;
            }

            public string getClima()
            {
                return clima;
            }

            public double getDemanda()
            {
                return demanda;
            }

            public double getCantidadComprada()
            {
                return cantidadComprada;
            }

            public double getGananciaVentas()
            {
                return gananciasVentas;
            }
            public double getGananciaVentasCementerio() 
            {
                return gananciasVentCem;
            }
            public double getCostoCompra() 
            {
                return costoCompra;
            }
            public double getCostoFaltante() 
            {
                return costoFaltante;
            }
            public double getCostoCompraSatisfacerDemanda()
            {
                return costoCompraSatisfacerDemanda;
            }

            internal double getVentasCementerio()
            {
                return ventasCementerio;
            }

            internal double getGananciaTotal()
            {
                return gananciaTotal;
            }

            internal double getRosasVendidasCementerioAcumulada()
            {
                return rosasVendidasCementerioAcumuladas;
            }

            internal void setRosasVendidasCementerioAcumulada(double acumRosas)
            {
                this.rosasVendidasCementerioAcumuladas = acumRosas;
            }
        }

        private string getClima(double randomClima)
        {
            if (randomClima < ProbDiaSoleado)
            {
                return "Soleado";
            }
            else
            {
                return "Nublado";
            }
        }

        private int getDemandaNublado(double randomDemanda)
        {
            if (randomDemanda < ProbDemDiaNublado1)
            {
                return Int32.Parse(dn1.Text) * 12;
            }
            else if (randomDemanda < ProbDemDiaNublado1 + ProbDemDiaNublado2)
            {
                return Int32.Parse(dn2.Text) * 12;
            }
            else if (randomDemanda < ProbDemDiaNublado1 + ProbDemDiaNublado2 + ProbDemDiaNublado3)
            {
                return Int32.Parse(dn3.Text) * 12;
            }
            else if (randomDemanda < ProbDemDiaNublado1 + ProbDemDiaNublado2 + ProbDemDiaNublado3 + ProbDemDiaNublado4)
            {
                return Int32.Parse(dn4.Text) * 12;
            }
            else
            {
                return Int32.Parse(dn5.Text) * 12;
            }
        }

        private int getDemandaSoleado(double randomDemanda)
        {
            if (randomDemanda < ProbDemDiaSoleado1)
            {
                return Int32.Parse(d1.Text) * 12;
            }
            else if (randomDemanda < ProbDemDiaSoleado1 + ProbDemDiaSoleado2)
            {
                return Int32.Parse(d2.Text) * 12;
            }
            else if (randomDemanda < ProbDemDiaSoleado1 + ProbDemDiaSoleado2 + ProbDemDiaSoleado3)
            {
                return Int32.Parse(d3.Text) * 12;
            }
            else
            {
                return Int32.Parse(d4.Text) * 12;
            }

        }

    }
}
