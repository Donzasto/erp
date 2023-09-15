<script lang="ts">
	let isAuthenticated = true;

	async function checkAuthenticated() {
		const respone = await fetch('/api/login');

		if (respone.ok) {
			isAuthenticated = true;
		} else {
			isAuthenticated = false;
		}
	}

	import { onMount } from 'svelte';

	onMount(async () => {
		checkAuthenticated();
	});
</script>

<h2>Main page</h2>

<a href="/admin-panel">admin-panel</a>

{#if isAuthenticated}
	<a href="/api/logout">logout</a>
{:else}
	<a href="/api/login">login</a>
{/if}
