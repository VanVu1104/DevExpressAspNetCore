using Microsoft.EntityFrameworkCore;
using DevExtremeAspNetCore.Models;

public class NoteNPLRepository
{
    private readonly AppDbContext _db;

    public NoteNPLRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(NoteNpl note)
    {
        _db.NoteNpls.Add(note);
        await _db.SaveChangesAsync();
    }

    public async Task<List<NoteNpl>> GetByNPLIdAsync(int nplId)
    {
        return await _db.NoteNpls
                        .Where(x => x.Idnpl == nplId)
                        .ToListAsync();
    }
}
