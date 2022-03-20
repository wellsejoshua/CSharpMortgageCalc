using CSharpMortgageCalc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpMortgageCalc.Helpers
{
  public class LoanHelper
  {
    public Loan GetPayments(Loan loan)
    {
      //claculate monthly payment
      loan.Payment = CalcPayment(loan.Amount, loan.Rate, loan.Term);

      //Create loop from 1 to term
      var balance = loan.Amount;
      var totalInterest = 0.0m;
      var monthlyInterest = 0.0m;
      var monthlyPrincipal = 0.0m;
      var monthlyRate = CalcMonthlyRate(loan.Rate);
      //loop over each month until I reach the term
      for (int month = 1; month <= loan.Term; month++)
      {
        monthlyInterest = CalcMonthlyInterest(balance, monthlyRate);
        totalInterest += monthlyInterest;
        monthlyPrincipal = loan.Payment - monthlyInterest;
        balance -= monthlyPrincipal;

        LoanPayment loanPayment = new();
        loanPayment.Month = month;
        loanPayment.Payment = loan.Payment;
        loanPayment.MonthlyPrincipal = monthlyPrincipal;
        loanPayment.MonthlyInterest = monthlyInterest;
        loanPayment.TotalInterest = totalInterest;
        loanPayment.Balance = balance;

        //push the object into the loan model
        loan.Payments.Add(loanPayment);
      }
      loan.TotalInterest = totalInterest;
      loan.TotalCost = loan.Amount + totalInterest;

      //return the loan to the view

      return loan;
    }

    private decimal CalcPayment(decimal amount, decimal rate, int term)
    {
      var monthlyRate = CalcMonthlyRate(rate); 
      var rateD = Convert.ToDouble(monthlyRate);
      var amountD = Convert.ToDouble(amount);

      var paymentD = (amountD * rateD) / (1 - Math.Pow(1 + rateD, -term));

      return Convert.ToDecimal(paymentD);

    }

    private decimal CalcMonthlyRate(decimal rate)
    {
      return rate / 1200;
    }

    private decimal CalcMonthlyInterest(decimal balance, decimal monthlyRate)
    {
      return balance * monthlyRate;

    }

  }
}
