﻿using CleanArchitecture.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Employees;

// Sealed => Miras (inheritance) alınmasını engellemek için kullanılır.
public sealed class Employee : Entity
{
    public string FirstName { get; set; } = default!; // bu kullanım bu değer null gelmeyecek anlamına gelir. Uyarı vermesini engeller.
    public string LastName { get; set; } = default!;
    public string FullName => string.Join(" ", FirstName, LastName); // User newlendiğinde db de gözükmeyecek ama benim kullanabileceğim bir field
    public DateTimeOffset BirthOfDate { get; set; }
    public PersonalInformation PersonalInformation { get; set; } = default!;
    public Address Address { get; set; } = default!;
}

