using System.Collections.Generic;
using MarcianoIO.Business.Notificacoes;

namespace MarcianoIO.Business.Intefaces
{
    public interface INotificador
    {
        bool TemNotificacao();
        List<Notificacao> ObterNotificacoes();
        void Handle(Notificacao notificacao);
    }
}