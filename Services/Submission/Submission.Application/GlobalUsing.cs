global using MediatR;
global using FluentValidation;

global using Articles.Abstractions;
global using Articles.Abstractions.Enums;
global using Blocks.Domain;
global using Blocks.Core.FluentValidation;

global using Submission.Application.Features._Shared;

global using Submission.Domain.Enums;
global using Submission.Domain.Entities;

global using Submission.Persistence.Repositories;

global using Blocks.MediatR.Behaviours;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Submission.Application.Features.CreateArticle;
global using System.Reflection;