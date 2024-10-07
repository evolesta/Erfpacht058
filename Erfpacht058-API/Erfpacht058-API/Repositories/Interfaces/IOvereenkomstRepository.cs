﻿using Erfpacht058_API.Models.Eigendom;
using Erfpacht058_API.Models.OvereenkomstNS;

namespace Erfpacht058_API.Repositories.Interfaces
{
    public interface IOvereenkomstRepository
    {
        Task<IEnumerable<Overeenkomst>> GetOvereenkomsten();
        Task<Overeenkomst> GetOvereenkomst(int id);
        Task<Overeenkomst> AddOvereenkomst(int eigendomId, Overeenkomst overeenkomst);
        Task<Overeenkomst> EditOvereenkomst(int id, OvereenkomstDto overeenkomstDto);
        Task<Overeenkomst> DeleteOvereenkomst(int id);
        Task<Eigendom> KoppelOvereenkomstAanEigendom(int eigendomId, int overeenkomstId);
        Task<Eigendom> GetEigendomById(int eigendomId);
    }
}