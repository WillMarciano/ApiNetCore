using System.Collections.Generic;
using MarcianoIO.Business.Notificacoes;

namespace MarcianoIO.Business.Interfaces
{
    public interface INotificador
    {
        bool TemNotificacao();
        List<Notificacao> ObterNotificacoes();
        void Handle(Notificacao notificacao);
    }
}