using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskHub.BuildingBlocks.Application.MultiTenancy;
using TaskHub.Modules.Tenancy.Application.Features.Tenants.Commands.AcceptMembership;
using TaskHub.Modules.Tenancy.Application.Features.Tenants.Commands.AddMember;
using TaskHub.Modules.Tenancy.Application.Features.Tenants.Commands.CreateTenant;
using TaskHub.Modules.Tenancy.Application.Features.Tenants.Commands.DeleteTenant;
using TaskHub.Modules.Tenancy.Application.Features.Tenants.Commands.UpdateTenant;
using TaskHub.Modules.Tenancy.Application.Features.Tenants.Queries.GetTenantById;
using TaskHub.Modules.Tenancy.Application.Features.Tenants.Queries.GetTenants;
using TaskHub.Modules.Tenancy.Domain.Enums;

namespace TaskHub.Modules.Tenancy.Api.Controllers;

[ApiController]
[Authorize]
[SkipTenantResolution]
[Route("api/tenants")]
public sealed class TenantsController : ControllerBase
{
    private readonly ISender _sender;

    public TenantsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GetTenantsResponse>>> GetAll(
        CancellationToken cancellationToken)
        => Ok(await _sender.Send(new GetTenantsQuery(), cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetTenantByIdResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new GetTenantByIdQuery(id),
            cancellationToken);

        return response is null ? NotFound() : Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<CreateTenantResponse>> Create(
        [FromBody] CreateTenantRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new CreateTenantCommand(
                request.Name,
                request.Slug,
                request.StorageMode,
                request.ConnectionString),
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = response.Id },
            response);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UpdateTenantResponse>> Update(
        Guid id,
        [FromBody] UpdateTenantRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new UpdateTenantCommand(
                id,
                request.Name,
                request.Slug,
                request.Status),
            cancellationToken);

        return response is null ? NotFound() : Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new DeleteTenantCommand(id),
            cancellationToken);

        return response is null ? NotFound() : NoContent();
    }

    [HttpPost("{id:guid}/members")]
    public async Task<ActionResult<AddMemberResponse>> AddMember(
        Guid id,
        [FromBody] AddMemberRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _sender.Send(
            new AddMemberCommand(id, request.UserId, request.Role),
            cancellationToken);

        return Ok(response);
    }

    [HttpPost("{id:guid}/members/accept")]
    public async Task<IActionResult> AcceptMembership(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _sender.Send(new AcceptMembershipCommand(id), cancellationToken);
        return NoContent();
    }
}

public sealed record CreateTenantRequest(
    string Name,
    string Slug,
    TenantStorageMode StorageMode,
    string? ConnectionString);

public sealed record UpdateTenantRequest(
    string Name,
    string Slug,
    TenantStatus Status);

public sealed record AddMemberRequest(
    Guid UserId,
    TenantMemberRole Role);
