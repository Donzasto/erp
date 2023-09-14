export const actions = {
    default: async ({ request }) => {
        const data = await request.formData();
        const login = data.get('login');
        const password = data.get('password');

    }
}