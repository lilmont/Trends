// System
global using System.Text.Json.Serialization;
global using System.Net;
global using System.Reflection;

// Microsoft
global using Microsoft.Extensions.Options;

// Third-party
global using MassTransit;
global using MongoDB.Bson.Serialization.Attributes;
global using MongoDB.Bson;
global using MongoDB.Driver;
global using FluentValidation;
global using MediatR;
global using Newtonsoft.Json;

// Trends
global using Twitter.Clone.Trends.Repositories;
global using Twitter.Clone.Trends.Models.Entities;
global using Twitter.Clone.Trends.Services;
global using Twitter.Clone.Trends.Persistence;
global using Twitter.Clone.Trends.EventHandler;
global using Twitter.Clone.Trends.Extensions;
global using Twitter.Clone.Trends.AppSettings;
global using Twitter.Clone.Trends.Models.Validators;
global using Twitter.Clone.Trends.Responses;

