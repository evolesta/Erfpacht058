describe('Test backend communicatie', () => {
    it('kan inloggen op backend en token ontvangen', () => {
        // Ga naar login
        cy.visit('https://erfpacht058.test:8080');

        cy.get('input[name="email"]').type('test@gebruiker.nl');
        cy.get('input[name="password"]').type('TEST123');
        cy.get('button[type="submit"]').click();

        cy.url().should('include', '/app');
    });
});